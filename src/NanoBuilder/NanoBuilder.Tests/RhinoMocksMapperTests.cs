using System;
using Xunit;
using FluentAssertions;
using Moq;

namespace NanoBuilder.Tests
{
   public class RhinoMocksMapperTests
   {
      [Fact]
      public void CreateForInstance_RhinoMocksAssemblyNotPresent_ThrowsTypeMapperException()
      {
         const string mockType = "Rhino.Mocks.MockRepository,Rhino.Mocks";

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetType( mockType ) ).Returns<Type>( null );

         var mapper = new RhinoMocksMapper( typeInspectorMock.Object );
         Action createForInstance = () => mapper.CreateForInterface( typeof( int ) );

         createForInstance.ShouldThrow<TypeMapperException>();
      }
   }
}
