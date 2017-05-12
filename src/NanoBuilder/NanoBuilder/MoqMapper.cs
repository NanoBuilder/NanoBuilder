using System;
using System.Linq;

namespace NanoBuilder
{
   public class MoqMapper : ITypeMapper
   {
      private readonly ITypeInspector _typeInspector;

      internal MoqMapper( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      public object CreateForInterface( Type type )
      {
         Type[] typeArguments = { type };

         var mockType = _typeInspector.GetType( "Moq.Mock`1,Moq" );

         if ( mockType == null )
         {
            throw new TypeMapperException();
         }

         var closedMockType = mockType.MakeGenericType( typeArguments );

         var mockObject = Activator.CreateInstance( closedMockType );

         var objectProperty = mockObject.GetType().GetProperties().Single( t => t.Name == "Object" && t.PropertyType == type );
         return objectProperty.GetMethod.Invoke( mockObject, null );
      }
   }
}
