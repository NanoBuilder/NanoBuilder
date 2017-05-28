using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   internal class FullParameterComposer<T> : IFullParameterComposer<T>
   {
      private readonly ConstructorInfo[] _constructors;
      private readonly IConstructorMatcher _constructorMatcher;
      private readonly IMapperFactory _mapperFactory;

      private readonly TypeMap _typeMap = new TypeMap();
      private ITypeMapper _interfaceMapper;

      public FullParameterComposer( ITypeInspector typeInspector, IConstructorMatcher constructorMatcher, IMapperFactory mapperFactory )
      {
         _constructors = typeInspector.GetConstructors( typeof( T ) );
         _constructorMatcher = constructorMatcher;
         _mapperFactory = mapperFactory;
      }

      private TParameterType Default<TParameterType>()
      {
         var type = typeof( TParameterType );
         TParameterType instance = default( TParameterType );

         if ( type.IsInterface && _interfaceMapper != null )
         {
            instance = (TParameterType) _interfaceMapper.CreateForInterface( typeof( TParameterType ) );
         }

         return instance;
      }

      public IParameterComposer<T> MapInterfacesTo<TMapperType>() where TMapperType : ITypeMapper
      {
         _interfaceMapper = _mapperFactory.Create<TMapperType>();

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

      public IFullParameterComposer<T> Skip<TParameterType>()
      {
         TParameterType instance = Default<TParameterType>();

         _typeMap.Add( instance );

         return this;
      }

      public T Build()
      {
         if ( SpecialType.CanAutomaticallyActivate<T>() || !_constructors.Any() )
         {
            return Default<T>();
         }

         var allMappedTypes = _typeMap.Flatten();

         var constructors = typeof( T ).GetConstructors();
         var constructor = _constructorMatcher.Match( constructors, allMappedTypes );
         var constructorParameters = constructor.GetParameters();

         var callingParameters = new object[constructorParameters.Length];

         for ( int index = 0; index < constructorParameters.Length; index++ )
         {
            if ( constructorParameters[index].ParameterType.IsInterface )
            {
               if ( _interfaceMapper != null )
               {
                  object instance = _interfaceMapper.CreateForInterface( constructorParameters[index].ParameterType );
                  callingParameters[index] = instance;
               }
            }

            var (parameter, found) = _typeMap.Get( constructorParameters[index].ParameterType );

            if ( found )
            {
               callingParameters[index] = parameter;
            }
         }

         return (T) constructor.Invoke( callingParameters );
      }
   }
}
