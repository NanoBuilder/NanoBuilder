using System;
using Xunit;
using FluentAssertions;

namespace NanoBuilder.AcceptanceTests
{
   public class BuildPrimitiveScenarios
   {
      [Fact]
      public void BuildingAPrimitiveShouldInitializeToDefault()
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
   }
}
