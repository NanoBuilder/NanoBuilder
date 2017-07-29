using System;
using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   internal class TypeInspector : ITypeInspector
   {
      public Type GetType( string typeName ) => Type.GetType( typeName );

      public ConstructorInfo[] GetConstructors( Type type )
         => type.GetTypeInfo().DeclaredConstructors.Where( ci => ci.IsPublic ).ToArray();
   }
}
