using Xunit;
using FluentAssertions;

namespace NanoBuilder.Tests
{
   public class EnumerableExtensionsTests
   {
      [Fact]
      public void Common_BothListsHaveOneElement_FindsTheCommonElement()
      {
         int[] one = { 1 };
         int[] two = { 1 };

         var common = one.Common( two );

         common.Should().Contain( 1 );
      }

      [Fact]
      public void Common_BothListsHaveDifferingElements_FindsNoCommonElements()
      {
         int[] one = { 1 };
         int[] two = { 2 };

         var common = one.Common( two );

         common.Should().BeEmpty();
      }

      [Fact]
      public void Common_OneListIsEmpty_FindsNoCommonElements()
      {
         int[] one = { };
         int[] two = { 2 };

         var common = one.Common( two );

         common.Should().BeEmpty();
      }

      [Fact]
      public void Common_BothListsHaveASharedElementButOneHasDuplicate_FindsOnlyTheCommonElements()
      {
         int[] one = { 1, 1 };
         int[] two = { 1, 2 };

         var common = one.Common( two );

         common.Should().HaveCount( 1 ).And.Contain( 1 );
      }

      [Fact]
      public void Common_BothListsDuplicateSharedElements_FindsTheCommonElements()
      {
         int[] one = { 1, 1, 2, 3 };
         int[] two = { 1, 1, 4, 5 };

         var common = one.Common( two );

         common.Should().HaveCount( 2 ).And.ContainInOrder( new[] { 1, 1 } );
      }
   }
}
