using Xunit;
using FluentAssertions;

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

         var parameter = typeMap.Get( typeof( int ) );

         parameter.Instance.Should().Be( expectedValue );
         parameter.WasFound.Should().BeTrue();
      }

      [Fact]
      public void Get_DoesNotHaveType_ReturnsNullValueAndNotFound()
      {
         var typeMap = new TypeMap();
         var parameter = typeMap.Get( typeof( int ) );

         parameter.Instance.Should().Be( null );
         parameter.WasFound.Should().BeFalse();
      }

      [Fact]
      public void Get_HasTwoTypes_ReturnsBothInOriginalOrder()
      {
         const int expectedValue1 = 1;
         const int expectedValue2 = 2;

         var typeMap = new TypeMap();
         typeMap.Add( expectedValue1 );
         typeMap.Add( expectedValue2 );

         var parameter1 = typeMap.Get( typeof( int ) );
         var parameter2 = typeMap.Get( typeof( int ) );

         parameter1.Instance.Should().Be( expectedValue1 );
         parameter1.WasFound.Should().BeTrue();

         parameter2.Instance.Should().Be( expectedValue2 );
         parameter2.WasFound.Should().BeTrue();
      }

      [Fact]
      public void Get_HasOneTypeButRequestsTwo_ReturnsNotFoundForSecondPair()
      {
         const int expectedValue = 1;

         var typeMap = new TypeMap();
         typeMap.Add( expectedValue );

         typeMap.Get( typeof( int ) );
         var parameter = typeMap.Get( typeof( int ) );

         parameter.WasFound.Should().BeFalse();
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
