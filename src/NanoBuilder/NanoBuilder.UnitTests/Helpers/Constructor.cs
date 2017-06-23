using System;
using Moq;

namespace NanoBuilder.UnitTests.Helpers
{
   internal static class Constructor
   {
      public static Mock<IConstructor> With( Type type )
      {
         var constructorMock = new Mock<IConstructor>();
         
         constructorMock.SetupGet( c => c.Type ).Returns( type );

         return constructorMock;
      }
   }
}
