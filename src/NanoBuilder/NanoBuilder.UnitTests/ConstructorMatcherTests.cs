using System;
using System.Linq;
using FluentAssertions;
using Moq;
using NanoBuilder.UnitTests.Helpers;
using Xunit;
using Array = NanoBuilder.UnitTests.Helpers.Array;

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

      [Fact]
      public void GetMatches_AddsTypeThatNoConstructorDefines_ThrowsParameterMappingException()
      {
         // Arrange

         var constructorMock = Constructor.With( typeof( int ) );
         var constructors = Array.From( constructorMock.Object );

         // Act

         var constructorMatcher = new ConstructorMatcher( constructors );

         Action add = () => constructorMatcher.Add( "A string" );

         // Assert

         add.ShouldThrow<ParameterMappingException>();
      }

      [Fact]
      public void GetMatches_AddsTypeThatMatchesTheOneConstructor_ReturnsTheMatch()
      {
         // Arrange

         var constructorMock = Constructor.With( typeof( int ) );
         var constructors = Array.From( constructorMock.Object );

         // Act

         var constructorMatcher = new ConstructorMatcher( constructors );

         constructorMatcher.Add( 5 );

         var matches = constructorMatcher.GetMatches();

         // Assert

         matches.Should().HaveCount( 1 ).And.Contain( constructorMock.Object );
      }

      [Fact]
      public void GetMatches_AddsTypeThatMatchesTwoConstructors_ThrowsAmbiguousConstructorException()
      {
         // Arrange

         var constructorMock1 = Constructor.With( typeof( int ) );
         var constructorMock2 = Constructor.With( typeof( int ), typeof( int ) );
         var constructors = Array.From( constructorMock1.Object, constructorMock2.Object );

         // Act

         var constructorMatcher = new ConstructorMatcher( constructors );

         constructorMatcher.Add( 5 );

         Action getMatches = () => constructorMatcher.GetMatches();

         // Assert

         getMatches.ShouldThrow<AmbiguousConstructorException>();
      }
   }
}
