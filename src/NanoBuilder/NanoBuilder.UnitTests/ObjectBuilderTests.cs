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
      public void Build_BuildingAnInt_ReturnsDefaultInt()
      {
         int value = ObjectBuilder.For<int>().Build();

         value.Should().Be( default( int ) );
      }

      [Fact]
      public void Build_BuildingAString_ReturnsDefaultString()
      {
         string value = ObjectBuilder.For<string>().Build();

         value.Should().Be( default( string ) );
      }

      [Fact]
      public void Build_BuildingAGuidWithParameter_ReturnsTheConstructedObject()
      {
         const string guidString = "86273ea7-b89d-45c2-a4f9-34f005e555da";

         var guid = ObjectBuilder.For<Guid>()
            .With( guidString )
            .Build();

         guid.ToString().Should().Be( guidString );
      }

      [Fact]
      public void Build_LeavesOneParameterUnmapped_UnmappedParameterIsDefaultValue()
      {
         var eventArgs = ObjectBuilder.For<ProgressChangedEventArgs>()
            .With( 80 )
            .Build();

         eventArgs.UserState.Should().Be( default( object ) );
      }

      [Fact]
      public void Build_BuildingAnExceptionWithComplexConstructor_ReturnsTheConstructedObject()
      {
         var innerException = new OverflowException();

         var exception = ObjectBuilder.For<Exception>()
            .With<Exception>( innerException )
            .Build();

         exception.InnerException.Should().Be( innerException );
      }

      [Fact]
      public void Build_BuildsVertexThatTakesTwoIntsButPassesOne_OnlyTheFirstIntIsUsed()
      {
         const int value = 5;

         var vertex = ObjectBuilder.For<Vertex>()
            .With( value )
            .Build();

         vertex.X.Should().Be( value );
         vertex.Y.Should().Be( default( int ) );
      }

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
