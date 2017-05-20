using System;
using Xunit;
using FluentAssertions;
using Moq;
using NSubstitute.Core;
using Rhino.Mocks.Interfaces;
using NanoBuilder.MockingTests.Stubs;

namespace NanoBuilder.MockingTests
{
   public class ObjectBuilderTests
   {
      [Fact]
      public void MapInterfacesTo_MultipleMappersAreUsed_ThrowsMapperException()
      {
         Action mapInterfacesTo = () => ObjectBuilder.For<Logger>()
            .MapInterfacesTo<MoqMapper>()
            .MapInterfacesTo<RhinoMocksMapper>();

         mapInterfacesTo.ShouldThrow<MapperException>();
      }

      [Fact]
      public void Build_OmitsInterfaceParameter_OmittedInterfaceBecomesAMoqOfThatInterface()
      {
         var logger = ObjectBuilder.For<Logger>()
            .MapInterfacesTo<MoqMapper>()
            .Build();

         Mock.Get( logger.FileSystem ).Should().BeOfType<Mock<IFileSystem>>();
      }

      [Fact]
      public void Build_OmitsInterfaceParameter_OmittedInterfaceBecomesARhinoMock()
      {
         var logger = ObjectBuilder.For<Logger>()
            .MapInterfacesTo<RhinoMocksMapper>()
            .Build();

         logger.FileSystem.GetType().GetInterfaces().Should().Contain( typeof( IMockedObject ) );
      }

      [Fact]
      public void Build_OmitsInterfaceParameter_OmittedInterfaceBecomesAnNSubstituteMock()
      {
         var logger = ObjectBuilder.For<Logger>()
            .MapInterfacesTo<NSubstituteMapper>()
            .Build();

         logger.FileSystem.GetType().GetInterfaces().Should().Contain( typeof( ICallRouter ) );
      }
   }
}
