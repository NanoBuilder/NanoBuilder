using System;
using System.Linq;

namespace NanoBuilder
{
   internal static class SpecialType
   {
      private static readonly Type[] _automaticTypes =
      {
         typeof( string )
      };

      public static bool CanAutomaticallyActivate<T>() => _automaticTypes.Contains( typeof( T ) );
   }
}
