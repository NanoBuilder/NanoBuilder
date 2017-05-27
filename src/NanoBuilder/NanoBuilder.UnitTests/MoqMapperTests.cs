using System;
using Xunit;
using FluentAssertions;
using Moq;

namespace NanoBuilder.UnitTests
{
   public class MoqMapperTests
   {
      [Fact]
      public void CreateForInstance_MoqAssemblyNotPresent_ThrowsTypeMapperException()
      {
         const string mockType = "Moq.Mock`1,Moq";

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetType( mockType ) ).Returns<Type>( null );

         var moqMapper = new MoqMapper( typeInspectorMock.Object );
         Action createForInstance = () => moqMapper.CreateForInterface( typeof( int ) );

         createForInstance.ShouldThrow<TypeMapperException>();
      }
   }
}
