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
      public void Build_BuildingAUriWithParameter_ReturnsTheConstructedObject()
      {
         const string address = "http://google.com";

         var uri = ObjectBuilder<Uri>.Create().With( () => address ).Build();

         uri.OriginalString.Should().Be( address );
      }
   }
}
