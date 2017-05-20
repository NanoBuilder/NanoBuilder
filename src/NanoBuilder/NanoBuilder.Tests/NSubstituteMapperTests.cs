using System;
using Xunit;
using FluentAssertions;
using Moq;

namespace NanoBuilder.Tests
{
   public class NSubstituteMapperTests
   {
      [Fact]
      public void CreateForInstance_NSubstituteAssemblyNotPresent_ThrowsTypeMapperException()
      {
         const string mockType = "NSubstitute.Substitute,NSubstitute";

         var typeInspectorMock = new Mock<ITypeInspector>();
         typeInspectorMock.Setup( ti => ti.GetType( mockType ) ).Returns<Type>( null );

         var mapper = new NSubstituteMapper( typeInspectorMock.Object );
         Action createForInstance = () => mapper.CreateForInterface( typeof( int ) );

         createForInstance.ShouldThrow<TypeMapperException>();
      }
   }
}
