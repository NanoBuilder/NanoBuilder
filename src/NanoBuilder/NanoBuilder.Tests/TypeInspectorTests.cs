using Xunit;
using FluentAssertions;

namespace NanoBuilder.Tests
{
   public class TypeInspectorTests
   {
      [Fact]
      public void GetType_PassesValidTypeName_GetsTheRightType()
      {
         var typeInspector = new TypeInspector();

         var type = typeInspector.GetType( "System.Int32" );

         type.Should().Be( typeof( int ) );
      }

      [Fact]
      public void GetType_PassesInvalidTypeName_ReturnsNull()
      {
         var typeInspector = new TypeInspector();

         var type = typeInspector.GetType( "System.NotAThing" );

         type.Should().BeNull();
      }
   }
}
