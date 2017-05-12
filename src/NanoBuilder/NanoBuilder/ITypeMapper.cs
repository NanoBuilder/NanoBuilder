using System;

namespace NanoBuilder
{
   public interface ITypeMapper
   {
      object CreateForInterface( Type type );
   }
}
