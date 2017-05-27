using System;
using System.Linq;
using FluentAssertions;
using NanoBuilder.Stubs;
using Xunit;

namespace NanoBuilder.UnitTests
{
   public class ConstructorMatcherTests
   {
      [Fact]
      public void Match_MapsOneIntButConstructorTakesTwoInts_FindsConstructor()
      {
         var constructorMatcher = new ConstructorMatcher();

         var constructors = typeof( Vertex ).GetConstructors();
         var types = new[] { typeof( int ) };

         var constructor = constructorMatcher.Match( constructors, types );

         var parameterTypes = constructor.GetParameters().Select( pi => pi.ParameterType );
         parameterTypes.Should().HaveCount( 2 ).And.ContainInOrder( typeof( int ), typeof( int ) );
      }

      [Fact]
      public void Match_HasOneIntButConstructorHasMultipleIntConstructors_ThrownExceptionIndicatesAmbiguousConstructors()
      {
         var constructorMatcher = new ConstructorMatcher();

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
         var constructorMatcher = new ConstructorMatcher();

         var constructors = typeof( TimeSpan ).GetConstructors();
         var types = new[] { typeof( string ) };

         Action match = () => constructorMatcher.Match( constructors, types );

         match.ShouldThrow<AmbiguousConstructorException>();
      }
   }
}
