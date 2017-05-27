using System;
using System.ComponentModel;
using Xunit;
using FluentAssertions;
using Moq;
using NanoBuilder.Stubs;

namespace NanoBuilder.UnitTests
{
   public class ObjectBuilderTests
   {
      [Fact]
      public void Skip_SkipsFirstConstructorButMapsSecond_SecondParameterIsSetButNotFirst()
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
      public void Skip_SkipsFirstConstructorButMapsSecondWithMoqMapper_SecondParameterIsSetToMockType()
      {
         var fileSystemMock = new Mock<IFileSystem>();

         var fileSystemAggregator = ObjectBuilder.For<FileSystemAggregator>()
            .MapInterfacesTo<MoqMapper>()
            .Skip<IFileSystem>()
            .With( fileSystemMock.Object )
            .Build();

         Mock.Get( fileSystemAggregator.FileSystem ).Should().BeOfType<Mock<IFileSystem>>();
         fileSystemAggregator.FileSystem2.Should().Be( fileSystemMock.Object );
      }

      [Fact]
      public void With_NoConstructorAcceptsTheParameterType_ThrowsParameterMappingException()
      {
         Action with = () => ObjectBuilder.For<Version>().With( 100.0 );

         with.ShouldThrow<ParameterMappingException>();
      }
   }
}
