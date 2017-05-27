using System;
using Xunit;
using FluentAssertions;
using Moq;

namespace NanoBuilder.UnitTests
{
   public class FakeItEasyMapperTests
   {
      [Fact]
      public void CreateForInstance_FakeItEasyAssemblyNotPresent_ThrowsTypeMapperException()
      {
         const string mockType = "FakeItEasy.A,FakeItEasy";

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetType( mockType ) ).Returns<Type>( null );

         var fakeItEasyMapper = new FakeItEasyMapper( typeInspectorMock.Object );
         Action createForInstance = () => fakeItEasyMapper.CreateForInterface( typeof( int ) );

         createForInstance.ShouldThrow<TypeMapperException>();
      }
   }
}
