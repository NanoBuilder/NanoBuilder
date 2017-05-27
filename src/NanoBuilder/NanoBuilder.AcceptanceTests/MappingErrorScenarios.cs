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
   }
}
