using Xunit;
using FluentAssertions;

namespace NanoBuilder.UnitTests
{
   public class TypeActivatorTests
   {
      [Fact]
      public void Default_GettingDefaultInt_GetsIntDefault()
      {
         var typeActivator = new TypeActivator();

         int intValue = typeActivator.Default<int>();

         intValue.Should().Be( default( int ) );
      }

      [Fact]
      public void Default_HasNoInterfaceMapper_MapsInterfaceToNull()
      {
         var typeActivator = new TypeActivator();

         var interfaceValue = typeActivator.Default<ITypeInspector>();

         interfaceValue.Should().BeNull();
      }
   }
}
