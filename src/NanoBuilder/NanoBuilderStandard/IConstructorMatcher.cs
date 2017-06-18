using System;
using System.Reflection;

namespace NanoBuilder
{
   internal interface IConstructorMatcher
   {
      ConstructorInfo Match( ConstructorInfo[] constructors, Type[] types );
   }
}
