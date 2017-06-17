using System;
using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   internal class MapperFactory : IMapperFactory
   {
      private readonly ITypeInspector _typeInspector;

      public MapperFactory( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      public T Create<T>() where T: ITypeMapper
      {
         var constructor = typeof( T ).GetTypeInfo()
            .DeclaredConstructors
            .SingleOrDefault( ci => ci.GetParameters().Length == 1
               && ci.GetParameters()[0].ParameterType == typeof( ITypeInspector ) );

         if ( constructor == null )
         {
            throw new NanoBuilderException( Resources.NanoBuilderMessage );
         }

         return (T) constructor.Invoke( new object[] { _typeInspector } );
      }
   }
}
