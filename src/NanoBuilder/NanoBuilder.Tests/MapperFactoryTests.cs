using System;
using Xunit;
using FluentAssertions;
using Moq;
using NanoBuilder.Tests.Stubs;

namespace NanoBuilder.Tests
{
   public class MapperFactoryTests
   {
      [Fact]
      public void Create_CreateMoqMapper_CreatesSuccessfully()
      {
         var typeInspectorMock = new Mock<ITypeInspector>();
         var mapperFactory = new MapperFactory( typeInspectorMock.Object );

         Action create = () => mapperFactory.Create<MoqMapper>();

         create.ShouldNotThrow();
      }

      [Fact]
      public void Create_CreateRhinoMockMapper_CreatesSuccessfully()
      {
         var typeInspectorMock = new Mock<ITypeInspector>();
         var mapperFactory = new MapperFactory( typeInspectorMock.Object );

         Action create = () => mapperFactory.Create<RhinoMocksMapper>();

         create.ShouldNotThrow();
      }

      [Fact]
      public void Create_CreatNSubstituteMapper_CreatesSuccessfully()
      {
         var typeInspectorMock = new Mock<ITypeInspector>();
         var mapperFactory = new MapperFactory( typeInspectorMock.Object );

         Action create = () => mapperFactory.Create<NSubstituteMapper>();

         create.ShouldNotThrow();
      }

      [Fact]
      public void Create_CreatFakeItEasyMapper_CreatesSuccessfully()
      {
         var typeInspectorMock = new Mock<ITypeInspector>();
         var mapperFactory = new MapperFactory( typeInspectorMock.Object );

         Action create = () => mapperFactory.Create<FakeItEasyMapper>();

         create.ShouldNotThrow();
      }

      [Fact]
      public void Create_MapperDoesNotHaveTheInternalConstructor_ThrowsNanoBuilderException()
      {
         var typeInspectorMock = new Mock<ITypeInspector>();
         var mapperFactory = new MapperFactory( typeInspectorMock.Object );

         Action create = () => mapperFactory.Create<FaultyMapper>();

         create.ShouldThrow<NanoBuilderException>().Which.InnerException.Should().BeOfType<MissingMethodException>();
      }
   }
}
