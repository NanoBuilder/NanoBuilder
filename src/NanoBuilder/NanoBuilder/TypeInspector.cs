using System;

namespace NanoBuilder
{
   internal class TypeInspector : ITypeInspector
   {
      public Type GetType( string typeName ) => Type.GetType( typeName );
   }
}
