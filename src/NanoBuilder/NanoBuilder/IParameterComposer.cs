namespace NanoBuilder
{
   /// <summary>
   /// A class that can configure constructor parameters.
   /// </summary>
   /// <typeparam name="T">The type of object to build.</typeparam>
   public interface IParameterComposer<T>
   {
      /// <summary>
      /// Configures a parameter for the object's constructor.
      /// </summary>
      /// <typeparam name="TParameterType">The type of object for the constructor.</typeparam>
      /// <param name="instance">The object that is being mapped for the given type.</param>
      /// <returns>The same <see cref="FullParameterComposer{T}"/>.</returns>
      IFullParameterComposer<T> With<TParameterType>( TParameterType instance );

      /// <summary>
      /// Provides a default value for the given type, allowing you to "skip" mapping a
      /// parameter. This is useful when a constructor accepts multiple parameters of the
      /// same type, and you want to map some of them (but not all).
      /// </summary>
      /// <typeparam name="TParameterType">The type of object for the constructor.</typeparam>
      /// <returns>The same <see cref="FullParameterComposer{T}"/>.</returns>
      IFullParameterComposer<T> Skip<TParameterType>();

      /// <summary>
      /// Creates the instance with the configured constructor parameters.
      /// </summary>
      /// <returns>The object instance.</returns>
      T Build();
   }
}
