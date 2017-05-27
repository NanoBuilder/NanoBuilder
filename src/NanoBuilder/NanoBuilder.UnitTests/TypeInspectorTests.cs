using System.Linq;
using Xunit;
using FluentAssertions;
using NanoBuilder.Stubs;

namespace NanoBuilder.UnitTests
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

      [Fact]
      public void GetConstructors_PassesTypeWithConstructors_ReturnsConstructors()
      {
         var actualConstructors = typeof( Logger ).GetConstructors();

         var typeInspector = new TypeInspector();

         var constructors = typeInspector.GetConstructors( typeof( Logger ) );

         constructors.Should().HaveCount( 1 ).And.Contain( actualConstructors.Single() );
      }
   }
}
