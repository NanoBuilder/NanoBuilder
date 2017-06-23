using System;
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
   }
}
