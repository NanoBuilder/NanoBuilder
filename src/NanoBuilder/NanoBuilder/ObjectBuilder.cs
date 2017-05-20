namespace NanoBuilder
{
   /// <summary>
   /// Provides a way of creating objects by specifying arguments for the constructor. Relevant arguments
   /// are mapped and used to create the object; unmapped arguments are initialized to their defaults unless
   /// otherwise configured.
   /// </summary>
   public static class ObjectBuilder
   {
      /// <summary>
      /// Begins building an object of the specified type.
      /// </summary>
      /// <typeparam name="T">The type of object to build.</typeparam>
      /// <returns>An ObjectBuilder instance that can build the given type.</returns>
      public static ParameterComposer<T> For<T>()
      {
         var constructors = typeof( T ).GetConstructors();

         return new ParameterComposer<T>( constructors, new TypeInspector(), new ConstructorMatcher() );
      }
   }
}
