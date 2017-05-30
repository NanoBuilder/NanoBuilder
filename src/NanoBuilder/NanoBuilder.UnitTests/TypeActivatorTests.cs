using Xunit;
using FluentAssertions;
using Moq;
using NanoBuilder.Stubs;

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

      [Fact]
      public void Default_HasInterfaceMapper_MapsInterfaceWithMapper()
      {
         var fileSystemMock = new Mock<IFileSystem>();

         var typeMapperMock = new Mock<ITypeMapper>();
         typeMapperMock.Setup( tm => tm.CreateForInterface( typeof( IFileSystem ) ) ).Returns( fileSystemMock.Object );

         var typeActivator = new TypeActivator
         {
            TypeMapper = typeMapperMock.Object
         };

         var fileSystem = typeActivator.Default<IFileSystem>();

         Mock.Get( fileSystem ).Should().Be( fileSystemMock );
      }
   }
}
