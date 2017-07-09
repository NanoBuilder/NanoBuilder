using System;

namespace NanoBuilder
{
   internal class TypeEntry
   {
      public Type Type
      {
         get;
      }

      public object Instance
      {
         get;
      }

      public TypeEntry( Type type, object instance )
      {
         Type = type;
         Instance = instance;
      }
   }
}
