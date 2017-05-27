using System;

namespace NanoBuilder.Tests.Stubs
{
   internal class FaultyMapper : ITypeMapper
   {
      public object CreateForInterface( Type type )
      {
         throw new NotImplementedException();
      }
   }
}
