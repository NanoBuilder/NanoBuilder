using System;
using Xunit;
using FluentAssertions;

namespace NanoBuilder.Tests
{
   public class ObjectBuilderTests
   {
      [Fact]
      public void Build_BuildingAnObject_ReturnsBuilderForThatType()
      {
         var builder = ObjectBuilder<string>.Create();

         builder.Should().BeOfType<ObjectBuilder<string>>();
      }

      [Fact]
      public void Build_BuildingAnInt_ReturnsDefaultInt()
      {
         int value = ObjectBuilder<int>.Create().Build();

         value.Should().Be( default( int ) );
      }

      [Fact]
      public void Build_BuildingAString_ReturnsDefaultString()
      {
         string value = ObjectBuilder<string>.Create().Build();

         value.Should().Be( default( string ) );
      }

      [Fact]
      public void Build_BuildingAGuidWithParameter_ReturnsTheConstructedObject()
      {
         const string guidString = "86273ea7-b89d-45c2-a4f9-34f005e555da";

         var guid = ObjectBuilder<Guid>.Create().With( () => guidString ).Build();

         guid.ToString().Should().Be( guidString );
      }

      [Fact]
      public void Build_BuildingAnExceptionWithComplexConstructor_ReturnsTheConstructedObject()
      {
         var innerException = new OverflowException();

         var exception = ObjectBuilder<Exception>.Create()
            .With<Exception>( () => innerException )
            .Build();

         exception.InnerException.Should().Be( innerException );
      }

      [Fact]
      public void Build_BuildingAVersionWithAmbiguousParameters_ThrowsAmbiguousConstructorException()
      {
         const int ambiguousInteger = 5;

         Action build = () => ObjectBuilder<Version>.Create().With( () => ambiguousInteger ).Build();

         build.ShouldThrow<AmbiguousConstructorException>();
      }

      [Fact]
      public void Build_BuildingAVersionWithAmbiguousParameters_ThrownExceptionIndicatesAmbiguousConstructors()
      {
         const int ambiguousInteger = 5;

         Action build = () => ObjectBuilder<Version>.Create().With( () => ambiguousInteger ).Build();

         build.ShouldThrow<AmbiguousConstructorException>().Where( e =>
            e.Message.Contains( "Void .ctor(Int32, Int32, Int32, Int32)" ) &&
            e.Message.Contains( "Void .ctor(Int32, Int32, Int32)" ) &&
            e.Message.Contains( "Void .ctor(Int32, Int32)" ) );
      }
   }
}
