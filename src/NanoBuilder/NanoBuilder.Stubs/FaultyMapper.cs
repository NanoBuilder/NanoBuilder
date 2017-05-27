using System;

namespace NanoBuilder.Stubs
{
   public class FaultyMapper : ITypeMapper
   {
      public object CreateForInterface( Type type )
      {
         throw new NotImplementedException();
      }
   }
}
