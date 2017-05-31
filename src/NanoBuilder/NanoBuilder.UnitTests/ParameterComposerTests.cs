using System;
using Xunit;
using FluentAssertions;
using Moq;
using NanoBuilder.Stubs;

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
         var typeActivatorMock = new Mock<ITypeActivator>();

         // Act

         var parameterComposer = new FullParameterComposer<int>( typeInspectorMock.Object, null, mapperFactoryMock.Object, typeActivatorMock.Object, null );

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

         var parameterComposer = new FullParameterComposer<Logger>( typeInspectorMock.Object, null, null, null, null );

         Action with = () => parameterComposer.With( fileSystemMock );

         // Assert

         string message = Resources.ParameterMappingWithMockMessage.Replace( "{0}", "*" );
         with.ShouldThrow<ParameterMappingException>().And.Message.Should().Match( message );
      }

      [Fact]
      public void Skip_SkipsObject_ObjectIsSkipped()
      {
         // Arrange

         var skipObject = new object();

         var typeInspectorMock = new Mock<ITypeInspector>();

         var typeActivatorMock = new Mock<ITypeActivator>();
         typeActivatorMock.Setup( ta => ta.Default<object>() ).Returns( skipObject );

         var typeMapMock = new Mock<ITypeMap>();

         // Act

         var composer = new FullParameterComposer<Logger>( typeInspectorMock.Object, null, null, typeActivatorMock.Object, typeMapMock.Object );

         composer.Skip<object>();

         // Assert

         typeMapMock.Verify( tm => tm.Add( skipObject ), Times.Once() );
      }

      [Fact]
      public void Skip_DoesntMatterWhatItSkips_ReturnsItself()
      {
         // Arrange

         var typeInspectorMock = new Mock<ITypeInspector>();
         var typeActivatorMock = new Mock<ITypeActivator>();
         var typeMapMock = new Mock<ITypeMap>();

         // Act

         var composer = new FullParameterComposer<Logger>( typeInspectorMock.Object, null, null, typeActivatorMock.Object, typeMapMock.Object );

         var returnedComposer = composer.Skip<object>();

         // Assert

         returnedComposer.Should().Be( composer );
      }
   }
}
