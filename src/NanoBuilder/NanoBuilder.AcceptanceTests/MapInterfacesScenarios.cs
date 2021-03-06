﻿using Xunit;
using FluentAssertions;
using Moq;
using NSubstitute.Core;
using Rhino.Mocks.Interfaces;
using FakeItEasy.Creation;
using NanoBuilder.Stubs;

namespace NanoBuilder.AcceptanceTests
{
   public class MapInterfacesScenarios
   {
      [Fact]
      public void CanMapInterfacesAndThenStillMapParameters()
      {
         const int cacheSize = 256;

         var fileSystemCache = ObjectBuilder.For<FileSystemCache>()
            .MapInterfacesWith<MoqMapper>()
            .With( cacheSize )
            .Build();

         Mock.Get( fileSystemCache.FileSystem ).Should().BeOfType<Mock<IFileSystem>>();
         fileSystemCache.CacheSize.Should().Be( cacheSize );
      }

      [Fact]
      public void CanMapInterfacesToMoqMocks()
      {
         var logger = ObjectBuilder.For<Logger>()
            .MapInterfacesWith<MoqMapper>()
            .Build();

         Mock.Get( logger.FileSystem ).Should().BeOfType<Mock<IFileSystem>>();
      }

      [Fact]
      public void CanMapInterfacesToRhinoMocks()
      {
         var logger = ObjectBuilder.For<Logger>()
            .MapInterfacesWith<RhinoMocksMapper>()
            .Build();

         logger.FileSystem.GetType().GetInterfaces().Should().Contain( typeof( IMockedObject ) );
      }

      [Fact]
      public void CanMapInterfacesToNSubstituteMocks()
      {
         var logger = ObjectBuilder.For<Logger>()
            .MapInterfacesWith<NSubstituteMapper>()
            .Build();

         logger.FileSystem.GetType().GetInterfaces().Should().Contain( typeof( ICallRouter ) );
      }

      [Fact]
      public void CanMapInterfacesToFakeItEasyMocks()
      {
         var logger = ObjectBuilder.For<Logger>()
            .MapInterfacesWith<FakeItEasyMapper>()
            .Build();

         logger.FileSystem.GetType().GetInterfaces().Should().Contain( typeof( ITaggable ) );
      }
   }
}
