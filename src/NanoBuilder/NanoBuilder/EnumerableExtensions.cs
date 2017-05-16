using System.Collections.Generic;
using System.Linq;

namespace NanoBuilder
{
   internal static class EnumerableExtensions
   {
      public static IEnumerable<T> Common<T>( this IEnumerable<T> first, IEnumerable<T> second )
      {
         var lookup1 = first.ToLookup( x => x );
         var lookup2 = second.ToLookup( x => x );

         return lookup1.SelectMany( l1s => lookup2[l1s.Key].Zip( l1s, ( l2, l1 ) => l1 ) );
      }
   }
}
