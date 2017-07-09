using System;
using Moq;

namespace NanoBuilder.UnitTests.Helpers
{
   internal static class Constructor
   {
      public static Mock<IConstructor> With( params Type[] types )
      {
         var constructorMock = new Mock<IConstructor>();

         constructorMock.SetupGet( c => c.ParameterTypes ).Returns( types );

         return constructorMock;
      }
   }
}
