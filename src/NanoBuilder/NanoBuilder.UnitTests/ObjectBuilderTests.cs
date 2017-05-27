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
      public void Build_ParameterIsAnInterface_InterfaceIsSetToTheMock()
      {
         var fileSystemMock = new Mock<IFileSystem>();

         var logger = ObjectBuilder.For<Logger>()
            .With( fileSystemMock.Object )
            .Build();

         logger.FileSystem.Should().Be( fileSystemMock.Object );
      }

      [Fact]
      public void Build_PassesTwoIdenticalParameters_ParametersAreMappedToParameterOrder()
      {
         const int year = 2017;
         const int month = 5;
         const int day = 15;

         var dateTime = ObjectBuilder.For<DateTime>()
            .With( year )
            .With( month )
            .With( day )
            .Build();

         dateTime.Year.Should().Be( year );
         dateTime.Month.Should().Be( month );
         dateTime.Day.Should().Be( day );
      }

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
