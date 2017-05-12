using System;
using System.ComponentModel;
using Xunit;
using FluentAssertions;
using Moq;
using NanoBuilder.Tests.Stubs;

namespace NanoBuilder.Tests
{
   public class ObjectBuilderTests
   {
      [Fact]
      public void Build_BuildingAnObject_ReturnsBuilderForThatType()
      {
         var builder = ObjectBuilder.For<string>();

         builder.Should().BeOfType<ObjectBuilder<string>>();
      }

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
            .With( () => guidString )
            .Build();

         guid.ToString().Should().Be( guidString );
      }

      [Fact]
      public void Build_LeavesOneParameterUnmapped_UnmappedParameterIsDefaultValue()
      {
         var eventArgs = ObjectBuilder.For<ProgressChangedEventArgs>()
            .With( () => 80 )
            .Build();

         eventArgs.UserState.Should().Be( default( object ) );
      }

      [Fact]
      public void Build_BuildingAnExceptionWithComplexConstructor_ReturnsTheConstructedObject()
      {
         var innerException = new OverflowException();

         var exception = ObjectBuilder.For<Exception>()
            .With<Exception>( () => innerException )
            .Build();

         exception.InnerException.Should().Be( innerException );
      }

      [Fact]
      public void Build_BuildingAVersionWithAmbiguousParameters_ThrowsAmbiguousConstructorException()
      {
         const int ambiguousInteger = 5;

         Action build = () => ObjectBuilder.For<Version>()
            .With( () => ambiguousInteger )
            .Build();

         build.ShouldThrow<AmbiguousConstructorException>();
      }

      [Fact]
      public void Build_BuildingAVersionWithAmbiguousParameters_ThrownExceptionIndicatesAmbiguousConstructors()
      {
         const int ambiguousInteger = 5;

         Action build = () => ObjectBuilder.For<Version>()
            .With( () => ambiguousInteger )
            .Build();

         build.ShouldThrow<AmbiguousConstructorException>().Where( e =>
            e.Message.Contains( "Void .ctor(Int32, Int32, Int32, Int32)" ) &&
            e.Message.Contains( "Void .ctor(Int32, Int32, Int32)" ) &&
            e.Message.Contains( "Void .ctor(Int32, Int32)" ) );
      }

      [Fact]
      public void Build_BuildsVertexThatTakesTwoIntsButPassesOne_OnlyTheFirstIntIsUsed()
      {
         const int value = 5;

         var vertex = ObjectBuilder.For<Vertex>()
            .With( () => value )
            .Build();

         vertex.X.Should().Be( value );
         vertex.Y.Should().Be( default( int ) );
      }

      [Fact]
      public void Build_ParameterIsAnInterface_InterfaceIsSetToTheMock()
      {
         var fileSystemMock = new Mock<IFileSystem>();

         var logger = ObjectBuilder.For<Logger>()
            .With( () => fileSystemMock.Object )
            .Build();

         logger.FileSystem.Should().Be( fileSystemMock.Object );
      }
   }
}
