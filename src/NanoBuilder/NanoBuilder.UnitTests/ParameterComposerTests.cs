using System;
using FluentAssertions;
using Moq;
using NanoBuilder.Stubs;
using Xunit;

namespace NanoBuilder.UnitTests
{
   public class ParameterComposerTests
   {
      [Fact]
      public void MapInterfacesTo_PassesMoqMapper_UsesFactoryToCreateMapper()
      {
         // Arrange

         var typeInspectorMock = new Mock<ITypeInspector>();
         var mapperFactoryMock = new Mock<IMapperFactory>();

         // Act

         var parameterComposer = new FullParameterComposer<int>( typeInspectorMock.Object, null, mapperFactoryMock.Object );

         parameterComposer.MapInterfacesTo<MoqMapper>();
         
         // Assert

         mapperFactoryMock.Verify( mf => mf.Create<MoqMapper>(), Times.Once() );
      }

      [Fact]
      public void With_PassesMockInsteadOfMockObject_ThrowsWithHelpfulMessage()
      {
         // Arrange

         var typeInspectorMock = new Mock<ITypeInspector>();

         // Act

         var fileSystemMock = new Mock<IFileSystem>();

         var parameterComposer = new FullParameterComposer<Logger>( typeInspectorMock.Object, null, null );

         Action with = () => parameterComposer.With( fileSystemMock );

         // Assert

         string message = Resources.ParameterMappingWithMockMessage.Replace( "{0}", "*" );
         with.ShouldThrow<ParameterMappingException>().And.Message.Should().Match( message );
      }
   }
}
