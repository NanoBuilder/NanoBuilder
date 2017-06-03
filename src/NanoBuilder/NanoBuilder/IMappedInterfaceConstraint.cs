namespace NanoBuilder
{
   /// <summary>
   /// This allows you decide how interfaces types can be initialized by default.
   /// </summary>
   /// <typeparam name="T">The type of object to build.</typeparam>
   public interface IMappedInterfaceConstraint<T>
   {
      /// <summary>
      /// Configures how interface types should be initialized by default. 
      /// </summary>
      /// <typeparam name="TMapperType">The type of mapper to transform objects.</typeparam>
      /// <returns>The same <see cref="FullParameterComposer{T}"/>.</returns>
      IParameterComposer<T> MapInterfacesWith<TMapperType>() where TMapperType : ITypeMapper;
   }
}
