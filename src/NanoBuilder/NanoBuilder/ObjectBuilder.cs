using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   /// <summary>
   /// Provides a way of creating objects by specifying arguments for the constructor. Relevant arguments
   /// are mapped and used to create the object; unmapped arguments are initialized to their defaults unless
   /// otherwise configured.
   /// </summary>
   public static class ObjectBuilder
   {
      private static IEnumerable<IConstructor> GetConstructors( Type type )
         => type.GetTypeInfo().DeclaredConstructors.Select( ci => new ConstructorWrapper( ci ) );

      /// <summary>
      /// Begins building an object of the specified type.
      /// </summary>
      /// <typeparam name="T">The type of object to build.</typeparam>
      /// <returns>A builder that can put together the given type.</returns>
      public static IFullParameterComposer<T> For<T>()
      {
         var constructors = GetConstructors( typeof( T ) );

         var typeInspector = new TypeInspector();
         var constructorMatcher = new ConstructorMatcher( constructors );
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
