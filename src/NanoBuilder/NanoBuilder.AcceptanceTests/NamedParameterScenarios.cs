using System;
using Xunit;
using FluentAssertions;

namespace NanoBuilder.AcceptanceTests
{
   public class NamedParameterScenarios
   {
      [Fact]
      public void BuildingVersionWithNamedParametersThatDontNeedToMatchConstructorParameterNames()
      {
         var version = ObjectBuilder.For<Version>()
            .With( mjr => 1 )
            .With( mnr => 2 )
            .With( maint => 3 )
            .Build();

         version.Major.Should().Be( 1 );
         version.Minor.Should().Be( 2 );
         version.Build.Should().Be( 3 );
      }
   }
}
