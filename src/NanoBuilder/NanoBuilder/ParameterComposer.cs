using System;
using System.Collections.Generic;
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
      private readonly TypeMap _typeMap = new TypeMap();
      private readonly ITypeInspector _typeInspector;
      private ITypeMapper _interfaceMapper;

      internal ParameterComposer( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      /// <summary>
      /// Configures how interface types should be initialized by default. 
      /// </summary>
      /// <typeparam name="TMapperType">The type of mapper to transform objects.</typeparam>
      /// <returns>The same <see cref="ParameterComposer{T}"/>.</returns>
      public ParameterComposer<T> MapInterfacesTo<TMapperType>() where TMapperType : ITypeMapper
      {
         var constructor = typeof( TMapperType ).GetConstructors( BindingFlags.NonPublic | BindingFlags.Instance ).Single();
         _interfaceMapper = (ITypeMapper) constructor.Invoke( new object[] { _typeInspector } );

         return this;
      }

      /// <summary>
      /// Configures a parameter for the object's constructor.
      /// </summary>
      /// <typeparam name="TParameterType">The type of object for the constructor.</typeparam>
      /// <param name="parameterProvider">A <see cref="Func{TResult}"/> that provides the instance for this parameter.</param>
      /// <returns>The same <see cref="ParameterComposer{T}"/>.</returns>
      public ParameterComposer<T> With<TParameterType>( Func<TParameterType> parameterProvider )
      {
         var instance = parameterProvider();

         _typeMap.Add( instance );

         return this;
      }

      /// <summary>
      /// Creates the instance with the configured constructor parameters.
      /// </summary>
      /// <returns>The object instance.</returns>
      public T Build()
      {
         if ( typeof( T ) == typeof( string ) )
         {
            return default( T );
         }

         var constructors = typeof( T ).GetConstructors();

         if ( constructors.Length == 0 )
         {
            return default( T );
         }

         var allMappedTypes = _typeMap.Flatten();

         var constructorMatcher = new ConstructorMatcher();
         var constructor = constructorMatcher.Match( constructors, allMappedTypes );
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
