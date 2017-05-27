using FluentAssertions;
using Xunit;

namespace NanoBuilder.UnitTests
{
   public class TypeMapTests
   {
      [Fact]
      public void Get_HasInt_ReturnsValueAndWasFound()
      {
         const int expectedValue = 5;

         var typeMap = new TypeMap();
         typeMap.Add( expectedValue );

         var (value, wasFound) = typeMap.Get( typeof( int ) );

         value.Should().Be( expectedValue );
         wasFound.Should().BeTrue();
      }

      [Fact]
      public void Get_DoesNotHaveType_ReturnsNullValueAndNotFound()
      {
         var typeMap = new TypeMap();
         var (value, wasFound) = typeMap.Get( typeof( int ) );

         value.Should().Be( null );
         wasFound.Should().BeFalse();
      }

      [Fact]
      public void Get_HasTwoTypes_ReturnsBothInOriginalOrder()
      {
         const int expectedValue1 = 1;
         const int expectedValue2 = 2;

         var typeMap = new TypeMap();
         typeMap.Add( expectedValue1 );
         typeMap.Add( expectedValue2 );

         var (value1, wasfound1) = typeMap.Get( typeof( int ) );
         var (value2, wasfound2) = typeMap.Get( typeof( int ) );

         value1.Should().Be( expectedValue1 );
         wasfound1.Should().BeTrue();

         value2.Should().Be( expectedValue2 );
         wasfound2.Should().BeTrue();
      }

      [Fact]
      public void Get_HasOneTypeButRequestsTwo_ReturnsNotFoundForSecondPair()
      {
         const int expectedValue = 1;

         var typeMap = new TypeMap();
         typeMap.Add( expectedValue );

         typeMap.Get( typeof( int ) );
         var (_, wasfound) = typeMap.Get( typeof( int ) );

         wasfound.Should().BeFalse();
      }

      [Fact]
      public void Flatten_HasNoTypes_ReturnsEmptySet()
      {
         var typeMap = new TypeMap();

         var allElements = typeMap.Flatten();

         allElements.Should().BeEmpty();
      }

      [Fact]
      public void Flatten_HasOneType_ReturnsTheOneType()
      {
         var typeMap = new TypeMap();
         typeMap.Add( 5 );
         
         var allElements = typeMap.Flatten();

         allElements.Should().HaveCount( 1 ).And.Contain( typeof( int ) );
      }

      [Fact]
      public void Flatten_HasTwoOfTheSameType_ReturnsAllTypes()
      {
         var typeMap = new TypeMap();
         typeMap.Add( 5 );
         typeMap.Add( 2 );

         var allElements = typeMap.Flatten();

         allElements.Should().HaveCount( 2 ).And.Contain( new[] { typeof( int ), typeof( int ) } );
      }
   }
}
