using System;
using System.Linq;
using System.Reflection;

namespace NanoBuilder.Flow
{
   internal class FullParameterComposer<T> : IFullParameterComposer<T>
   {
      private readonly ConstructorInfo[] _constructors;
      private readonly IConstructorMatcher _constructorMatcher;
      private readonly IMapperFactory _mapperFactory;
      private readonly ITypeActivator _typeActivator;
      private readonly ITypeMap _typeMap;

      private ITypeMapper _interfaceMapper;

      public FullParameterComposer( ITypeInspector typeInspector,
         IConstructorMatcher constructorMatcher,
         IMapperFactory mapperFactory,
         ITypeActivator typeActivator,
         ITypeMap typeMap )
      {
         _constructors = typeInspector.GetConstructors( typeof( T ) );
         _constructorMatcher = constructorMatcher;
         _mapperFactory = mapperFactory;
         _typeActivator = typeActivator;
         _typeMap = typeMap;
      }

      public IParameterComposer<T> MapInterfacesWith<TMapperType>() where TMapperType : ITypeMapper
      {
         _interfaceMapper = _mapperFactory.Create<TMapperType>();

         _typeActivator.TypeMapper = _interfaceMapper;

         return this;
      }

      public IFullParameterComposer<T> With<TParameterType>( TParameterType instance )
      {
         var parameterMatches = from c in _constructors
                                from p in c.GetParameters()
                                where p.ParameterType == typeof( TParameterType )
                                select p.ParameterType;

         if ( !parameterMatches.Any() )
         {
            string message;

            if ( instance.GetType().Name == "Mock`1" )
            {
               message = string.Format( Resources.ParameterMappingWithMockMessage, typeof( T ) );
            }
            else
            {
               message = string.Format( Resources.ParameterMappingMessage, typeof( T ), typeof( TParameterType ) );
            }

            throw new ParameterMappingException( message );
         }

         _typeMap.Add( instance );

         return this;
      }

      public IFullParameterComposer<T> With<TParameterType>( Func<ParameterName, TParameterType> instanceProvider )
      {
         return this;
      }

      public IFullParameterComposer<T> Skip<TParameterType>()
      {
         TParameterType instance = _typeActivator.Default<TParameterType>();

         _typeMap.Add( instance );

         return this;
      }

      public T Build()
      {
         if ( SpecialType.CanAutomaticallyActivate<T>() || !_constructors.Any() )
         {
            return _typeActivator.Default<T>();
         }

         var allMappedTypes = _typeMap.Flatten();

         var constructor = _constructorMatcher.Match( _constructors, allMappedTypes );
         var constructorParameters = constructor.GetParameters();

         var callingParameters = new object[constructorParameters.Length];

         for ( int index = 0; index < constructorParameters.Length; index++ )
         {
            var typeInfo = constructorParameters[index].ParameterType.GetTypeInfo();

            if ( typeInfo.IsInterface )
            {
               if ( _interfaceMapper != null )
               {
                  object instance = _interfaceMapper.CreateForInterface( constructorParameters[index].ParameterType );
                  callingParameters[index] = instance;
               }
            }

            var parameter = _typeMap.Get( constructorParameters[index].ParameterType );

            if ( parameter.WasFound )
            {
               callingParameters[index] = parameter.Instance;
            }
         }

         return (T) constructor.Invoke( callingParameters );
      }
   }
}
