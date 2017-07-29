using System;
using Xunit;
using FluentAssertions;

namespace NanoBuilder.AcceptanceTests
{
   public class VariedConstructorScenarios
   {
      [Fact]
      public void CanBuildObjectWithManyIntegers()
      {
         var version = ObjectBuilder.For<Version>()
            .With( 1 )
            .With( 2 )
            .With( 3 )
            .Build();

         version.ToString().Should().Be( "1.2.3" );
      }

      [Fact]
      public void CanBuildObjectWithManyIntegersAndSkipTheMiddle()
      {
         var version = ObjectBuilder.For<Version>()
            .With( 1 )
            .Skip<int>()
            .With( 3 )
            .Build();

         version.ToString().Should().Be( "1.0.3" );
      }
   }
}
