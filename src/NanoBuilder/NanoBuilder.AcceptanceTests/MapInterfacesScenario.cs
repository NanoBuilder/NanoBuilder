using Xunit;
using FluentAssertions;
using Moq;
using NanoBuilder.Stubs;

namespace NanoBuilder.AcceptanceTests
{
   public class MapInterfacesScenario
   {
      [Fact]
      public void CanMapInterfacesAndThenStillMapParameters()
      {
         const int cacheSize = 256;

         var fileSystemCache = ObjectBuilder.For<FileSystemCache>()
            .MapInterfacesTo<MoqMapper>()
            .With( cacheSize )
            .Build();

         Mock.Get( fileSystemCache.FileSystem ).Should().BeOfType<Mock<IFileSystem>>();
         fileSystemCache.CacheSize.Should().Be( cacheSize );
      }
   }
}
