﻿using System;
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
   }
}
