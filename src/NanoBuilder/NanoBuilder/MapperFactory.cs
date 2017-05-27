using System;
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
         try
         {
            return (T) Activator.CreateInstance( typeof( T ), BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { _typeInspector }, null );
         }
         catch ( MissingMethodException ex )
         {
            throw new NanoBuilderException( Resources.NanoBuilderMessage, ex );
         }
      }
   }
}
