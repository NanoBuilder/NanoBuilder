using System;
using Xunit;
using FluentAssertions;
using Moq;

namespace NanoBuilder.Tests
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

         var parameterComposer = new ParameterComposer<int>( typeInspectorMock.Object, null, mapperFactoryMock.Object );

         parameterComposer.MapInterfacesTo<MoqMapper>();
         
         // Assert

         mapperFactoryMock.Verify( mf => mf.Create<MoqMapper>(), Times.Once() );
      }

      [Fact]
      public void MapInterfacesTo_SetsMapperTwice_ThrowsMapperException()
      {
         // Arrange

         var typeMapperMock = new Mock<ITypeMapper>();

         var typeInspectorMock = new Mock<ITypeInspector>();
         var mapperFactoryMock = new Mock<IMapperFactory>();
         mapperFactoryMock.Setup( mf => mf.Create<ITypeMapper>() ).Returns( typeMapperMock.Object );

         // Act

         var parameterComposer = new ParameterComposer<int>( typeInspectorMock.Object, null, mapperFactoryMock.Object );

         parameterComposer.MapInterfacesTo<ITypeMapper>();
         Action map = () => parameterComposer.MapInterfacesTo<ITypeMapper>();

         map.ShouldThrow<MapperException>();
      }
   }
}
