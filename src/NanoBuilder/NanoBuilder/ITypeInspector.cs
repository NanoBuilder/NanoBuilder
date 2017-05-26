using System;
using System.Reflection;

namespace NanoBuilder
{
   internal interface ITypeInspector
   {
      Type GetType( string typeName );

      ConstructorInfo[] GetConstructors( Type type );
   }
}
