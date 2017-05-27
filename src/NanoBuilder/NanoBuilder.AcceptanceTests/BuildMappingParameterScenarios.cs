using System;
using System.ComponentModel;
using Xunit;
using FluentAssertions;
using NanoBuilder.Stubs;

namespace NanoBuilder.AcceptanceTests
{
   public class BuildMappingParameterScenarios
   {
      [Fact]
      public void MappingFirstParameterButNotTheSecondWillUseItsDefaultValueForTheSecond()
      {
         var eventArgs = ObjectBuilder.For<ProgressChangedEventArgs>()
            .With( 80 )
            .Build();

         eventArgs.UserState.Should().Be( default( object ) );
      }

      [Fact]
      public void MappingFirstParameterWillSetTheFirstParameter()
      {
         var innerException = new OverflowException();

         var exception = ObjectBuilder.For<Exception>()
            .With<Exception>( innerException )
            .Build();

         exception.InnerException.Should().Be( innerException );
      }

      [Fact]
      public void MappingOnlyTheFirstParameterWillActuallySetTheFirstParameter()
      {
         const int value = 5;

         var vertex = ObjectBuilder.For<Vertex>()
            .With( value )
            .Build();

         vertex.X.Should().Be( value );
         vertex.Y.Should().Be( default( int ) );
      }
   }
}
