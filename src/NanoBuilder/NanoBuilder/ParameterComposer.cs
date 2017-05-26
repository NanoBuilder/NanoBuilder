using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   /// <summary>
   /// A class that can configure constructor parameters.
   /// </summary>
   /// <typeparam name="T">The type of object to build.</typeparam>
   public class ParameterComposer<T>
   {
      private readonly ConstructorInfo[] _constructors;
      private readonly ITypeInspector _typeInspector;
      private readonly IConstructorMatcher _constructorMatcher;

      private readonly TypeMap _typeMap = new TypeMap();
      private ITypeMapper _interfaceMapper;

      internal ParameterComposer( ConstructorInfo[] constructors, ITypeInspector typeInspector, IConstructorMatcher constructorMatcher )
      {
         _constructors = constructors;
         _typeInspector = typeInspector;
         _constructorMatcher = constructorMatcher;
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

      /// <summary>
      /// Configures how interface types should be initialized by default. 
      /// </summary>
      /// <typeparam name="TMapperType">The type of mapper to transform objects.</typeparam>
      /// <returns>The same <see cref="ParameterComposer{T}"/>.</returns>
      public ParameterComposer<T> MapInterfacesTo<TMapperType>() where TMapperType : ITypeMapper
      {
         if ( _interfaceMapper != null )
         {
            throw new MapperException( Resources.MapperMessage );
         }

         var constructor = typeof( TMapperType ).GetConstructors( BindingFlags.NonPublic | BindingFlags.Instance ).Single();

         _interfaceMapper = (ITypeMapper) constructor.Invoke( new object[] { _typeInspector } );

         return this;
      }

      /// <summary>
      /// Configures a parameter for the object's constructor.
      /// </summary>
      /// <typeparam name="TParameterType">The type of object for the constructor.</typeparam>
      /// <param name="instance">The object that is being mapped for the given type.</param>
      /// <returns>The same <see cref="ParameterComposer{T}"/>.</returns>
      public ParameterComposer<T> With<TParameterType>( TParameterType instance )
      {
         var parameterMatches = from c in _constructors
                                from p in c.GetParameters()
                                where p.ParameterType == typeof( TParameterType )
                                select p.ParameterType;

         if ( !parameterMatches.Any() )
         {
            string message = string.Format( Resources.ParameterMappingMessage, typeof( T ), typeof( TParameterType ) );
            throw new ParameterMappingException( message );
         }

         _typeMap.Add( instance );

         return this;
      }

      /// <summary>
      /// Provides a default value for the given type, allowing you to "skip" mapping a
      /// parameter. This is useful when a constructor accepts multiple parameters of the
      /// same type, and you want to map some of them (but not all).
      /// </summary>
      /// <typeparam name="TParameterType">The type of object for the constructor.</typeparam>
      /// <returns>The same <see cref="ParameterComposer{T}"/>.</returns>
      public ParameterComposer<T> Skip<TParameterType>()
      {
         TParameterType instance = Default<TParameterType>();

         _typeMap.Add( instance );

         return this;
      }

      /// <summary>
      /// Creates the instance with the configured constructor parameters.
      /// </summary>
      /// <returns>The object instance.</returns>
      public T Build()
      {
         if ( SpecialType.CanAutomaticallyActivate<T>() || !SpecialType.HasConstructors<T>() )
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

      /// <summary>
      /// Automatically builds the instance from the <see cref="ParameterComposer{T}"/>. Using this
      /// means a call to Build() is unnecessary.
      /// </summary>
      /// <param name="composer">
      /// The current <see cref="ParameterComposer{T}"/>. This is most useful when
      /// chaining together method calls.
      /// </param>
      public static implicit operator T( ParameterComposer<T> composer )
         => composer.Build();
   }
}
