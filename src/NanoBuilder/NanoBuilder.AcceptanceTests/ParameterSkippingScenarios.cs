using Xunit;
using FluentAssertions;
using Moq;
using NanoBuilder.Stubs;

namespace NanoBuilder.AcceptanceTests
{
   public class ParameterSkippingScenarios
   {
      [Fact]
      public void SkippingTheFirstButMappingTheSecondIntParameterWillSetTheSecond()
      {
         const int y = 123;

         var vertex = ObjectBuilder.For<Vertex>()
            .Skip<int>()
            .With( y )
            .Build();

         vertex.X.Should().Be( default( int ) );
         vertex.Y.Should().Be( y );
      }

      [Fact]
      public void SkippingAnInterfaceButSettingTheSecondWillDefaultTheFirstToTheRightMockType()
      {
         var fileSystemMock = new Mock<IFileSystem>();

         var fileSystemAggregator = ObjectBuilder.For<FileSystemAggregator>()
            .MapInterfacesWith<MoqMapper>()
            .Skip<IFileSystem>()
            .With( fileSystemMock.Object )
            .Build();

         Mock.Get( fileSystemAggregator.FileSystem ).Should().BeOfType<Mock<IFileSystem>>();
         fileSystemAggregator.FileSystem2.Should().Be( fileSystemMock.Object );
      }
   }
}
