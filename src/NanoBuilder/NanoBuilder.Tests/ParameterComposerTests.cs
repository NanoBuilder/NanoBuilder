using Xunit;
using Moq;

namespace NanoBuilder.Tests
{
   public class ParameterComposerTests
   {
      [Fact]
      public void Create_PassesMoqMapper_UsesFactoryToCreateMapper()
      {
         // Arrange

         var typeInspectorMock = new Mock<ITypeInspector>();
         var mapperFactoryMock = new Mock<IMapperFactory>();

         // Act

         var parameterComposer = new ParameterComposer<int>( typeInspectorMock.Object, null, mapperFactoryMock.Object );

         parameterComposer.MapInterfacesTo<MoqMapper>();
         
         // Assert

         mapperFactoryMock.Verify( mf => mf.Create<MoqMapper>(), Times.Once() );
      }
   }
}
