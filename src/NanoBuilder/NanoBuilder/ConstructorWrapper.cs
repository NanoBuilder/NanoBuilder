using System;
using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   internal class ConstructorWrapper : IConstructor
   {
      public Type[] ParameterTypes
      {
         get;
      }

      public ConstructorWrapper( ConstructorInfo constructorInfo )
      {
         ParameterTypes = constructorInfo.GetParameters().Select( pi => pi.ParameterType ).ToArray();
      }
   }
}
