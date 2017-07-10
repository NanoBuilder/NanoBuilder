using System;
using System.Linq;
using FluentAssertions;
using NanoBuilder.Stubs;
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
      public void GetMatches_SetupNoParameters_ThrowsArgumentException()
      {
         Action ctor = () => new ConstructorMatcher( Enumerable.Empty<IConstructor>() );

         ctor.ShouldThrow<ArgumentException>();
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

         matches.Should().Be( constructorMock.Object );
      }

      [Fact]
      public void GetMatches_MatchesTwoConstructorsButMatchesOnePerfectly_ReturnsThePerfectMatch()
      {
         // Arrange

         var constructorMock1 = Constructor.With( typeof( int ) );
         var constructorMock2 = Constructor.With( typeof( int ), typeof( int ) );
         var constructors = Array.From( constructorMock1.Object, constructorMock2.Object );

         // Act

         var constructorMatcher = new ConstructorMatcher( constructors );

         constructorMatcher.Add( 5 );

         // Assert

         var matches = constructorMatcher.GetMatches();

         matches.Should().Be( constructorMock1.Object );
      }

      [Fact]
      public void GetMatches_AddsTwoTypesThatMatchesConstructor_ReturnsTheMatch()
      {
         // Arrange

         var constructorMock1 = Constructor.With( typeof( int ) );
         var constructorMock2 = Constructor.With( typeof( int ), typeof( int ) );
         var constructors = Array.From( constructorMock1.Object, constructorMock2.Object );

         // Act

         var constructorMatcher = new ConstructorMatcher( constructors );

         constructorMatcher.Add( 5 );
         constructorMatcher.Add( 6 );

         // Assert

         var matches = constructorMatcher.GetMatches();

         matches.Should().Be( constructorMock2.Object );
      }

      [Fact]
      public void Match_MapsOneIntButConstructorTakesTwoInts_FindsConstructor()
      {
         var constructorMatcher = new ConstructorMatcherOld();

         var constructors = typeof( Vertex ).GetConstructors();
         var types = new[] { typeof( int ) };

         var constructor = constructorMatcher.Match( constructors, types );

         var parameterTypes = constructor.GetParameters().Select( pi => pi.ParameterType );
         parameterTypes.Should().HaveCount( 2 ).And.ContainInOrder( typeof( int ), typeof( int ) );
      }

      [Fact]
      public void Match_HasOneIntButConstructorHasMultipleIntConstructors_ThrownExceptionIndicatesAmbiguousConstructors()
      {
         var constructorMatcher = new ConstructorMatcherOld();

         var constructors = typeof( Version ).GetConstructors();
         var types = new[] { typeof( int ) };

         Action match = () => constructorMatcher.Match( constructors, types );

         match.ShouldThrow<AmbiguousConstructorException>().Where( e =>
            e.Message.Contains( "Void .ctor(Int32, Int32, Int32, Int32)" ) &&
            e.Message.Contains( "Void .ctor(Int32, Int32, Int32)" ) &&
            e.Message.Contains( "Void .ctor(Int32, Int32)" ) );
      }

      [Fact]
      public void Match_NoSuitableConstructorsFoundForMappedTypes_ThrowsAmbiguousConstructorException()
      {
         var constructorMatcher = new ConstructorMatcherOld();

         var constructors = typeof( TimeSpan ).GetConstructors();
         var types = new[] { typeof( string ) };

         Action match = () => constructorMatcher.Match( constructors, types );

         match.ShouldThrow<AmbiguousConstructorException>();
      }
   }
}
