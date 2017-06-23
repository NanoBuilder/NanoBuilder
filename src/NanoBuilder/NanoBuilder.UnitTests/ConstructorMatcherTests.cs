using System;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace NanoBuilder.UnitTests
{
   public class ConstructorMatcherTests
   {
      [Fact]
      public void Constructor_PassesNullConstructors_ThrowsArgumentException()
      {
         Action ctor = () => new ConstructorMatcher( null );

         ctor.ShouldThrow<ArgumentException>();
      }

      [Fact]
      public void GetMatches_SetupNoParameters_ReturnsEmptySet()
      {
         var constructorMatcher = new ConstructorMatcher( Enumerable.Empty<IConstructor>() );

         var matches = constructorMatcher.GetMatches();

         matches.Should().BeEmpty();
      }
   }
}
