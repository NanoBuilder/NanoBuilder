using System;

namespace NanoBuilder
{
   /// <summary>
   /// A specific builder that can be configured to create the specified object type.
   /// </summary>
   /// <typeparam name="T">The type of object to build.</typeparam>
   public interface IObjectBuilder<T>
   {
      /// <summary>
      /// Configures how interface types should be initialized by default. 
      /// </summary>
      /// <typeparam name="TMapperType">The type of mapper to transform objects.</typeparam>
      /// <returns>The same <see cref="IObjectBuilder{T}"/>.</returns>
      IObjectBuilder<T> MapInterfacesTo<TMapperType>() where TMapperType : ITypeMapper;

      /// <summary>
      /// Configures a parameter for the object's constructor.
      /// </summary>
      /// <typeparam name="TParameterType">The type of object for the constructor.</typeparam>
      /// <param name="parameterProvider">A <see cref="Func{TResult}"/> that provides the instance for this parameter.</param>
      /// <returns>The same <see cref="IObjectBuilder{T}"/>.</returns>
      IObjectBuilder<T> With<TParameterType>( Func<TParameterType> parameterProvider );

      /// <summary>
      /// Creates the instance with the configured constructor parameters.
      /// </summary>
      /// <returns>The object instance.</returns>
      T Build();
   }
}