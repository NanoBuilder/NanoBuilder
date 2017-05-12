using System;
using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   public class MoqMapper : ITypeMapper
   {
      public object CreateForInterface( Type type )
      {
         Type[] typeArguments = { type };

         var mockType = Type.GetType( "Moq.Mock`1,Moq" );
         var closedMockType = mockType.MakeGenericType( typeArguments );

         var mockObject = Activator.CreateInstance( closedMockType );

         var objectProperty = mockObject.GetType().GetProperties().Single( t => t.Name == "Object" && t.PropertyType == type );
         return objectProperty.GetMethod.Invoke( mockObject, null );
      }
   }
}
