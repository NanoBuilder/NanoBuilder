using Xunit;
using FluentAssertions;

namespace NanoBuilder.Tests
{
   public class ObjectBuilderTests
   {
      [Fact]
      public void Build_BuildingAnObject_ReturnsBuilderForThatType()
      {
         var builder = ObjectBuilder<string>.Create();

         builder.Should().BeOfType<ObjectBuilder<string>>();
      }

      [Fact]
      public void Build_BuildingAnInt_ReturnsDefaultInt()
      {
         int value = ObjectBuilder<int>.Create().Build();

         value.Should().Be( default( int ) );
      }
   }
}
