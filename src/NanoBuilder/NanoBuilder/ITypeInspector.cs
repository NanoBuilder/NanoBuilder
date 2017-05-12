using System;

namespace NanoBuilder
{
   internal interface ITypeInspector
   {
      Type GetType( string typeName );
   }
}
