using NanoBuilder.Flow;

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
      /// <returns>A builder that can put together the given type.</returns>
      public static FullParameterComposer<T> For<T>()
      {
         var typeInspector = new TypeInspector();
         var constructorMatcher = new ConstructorMatcher();
         var mapperFactory = new MapperFactory( typeInspector );
         var typeActivator = new TypeActivator();
         var typeMap = new TypeMap();

         return new FullParameterComposer<T>( typeInspector,
            constructorMatcher,
            mapperFactory,
            typeActivator,
            typeMap );
      }
   }
}
