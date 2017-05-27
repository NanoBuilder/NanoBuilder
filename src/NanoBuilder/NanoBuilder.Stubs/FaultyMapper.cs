using System;

namespace NanoBuilder.Stubs
{
   internal class FaultyMapper : ITypeMapper
   {
      public object CreateForInterface( Type type )
      {
         throw new NotImplementedException();
      }
   }
}
