using System;
using Xunit;
using FluentAssertions;

namespace NanoBuilder.AcceptanceTests
{
   public class MappingErrorScenarios
   {
      [Fact]
      public void With_NoConstructorAcceptsTheParameterType_ThrowsParameterMappingException()
      {
         Action with = () => ObjectBuilder.For<Version>().With( 100.0 );

         with.ShouldThrow<ParameterMappingException>();
      }

      [Fact]
      public void With_NoConstructorAcceptsTheNamedParameterType_ThrowsParameterMappingException()
      {
         Action build = () => ObjectBuilder.For<Version>().With( someDouble => 100.0 );

         build.ShouldThrow<ParameterMappingException>();
      }

   }
}
