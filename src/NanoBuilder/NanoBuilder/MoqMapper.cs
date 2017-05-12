using System;
using System.Linq;

namespace NanoBuilder
{
   /// <summary>
   /// Represents a mapper that can transform a type to a Moq.Mock version of itself. The Moq
   /// library must be present!
   /// </summary>
   public class MoqMapper : ITypeMapper
   {
      private readonly ITypeInspector _typeInspector;

      internal MoqMapper( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      /// <summary>
      /// Creates an Moq.Mock instance that maps to the given type.
      /// </summary>
      /// <param name="type">The <see cref="Type"/> of object that needs to be mapped.</param>
      /// <returns>An object that represents the mapped type.</returns>
      /// <exception cref="TypeMapperException">
      /// Thrown if the Mock type can't be found. This is likely from not referencing the Moq library.
      /// </exception>
      public object CreateForInterface( Type type )
      {
         Type[] typeArguments = { type };

         var mockType = _typeInspector.GetType( "Moq.Mock`1,Moq" );

         if ( mockType == null )
         {
            throw new TypeMapperException( Resources.TypeMapperMessage );
         }

         var closedMockType = mockType.MakeGenericType( typeArguments );
         var mockObject = Activator.CreateInstance( closedMockType );

         var objectProperty = mockObject.GetType().GetProperties().Single( t => t.Name == "Object" && t.PropertyType == type );
         return objectProperty.GetMethod.Invoke( mockObject, null );
      }
   }
}
