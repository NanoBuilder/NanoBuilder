using System;
using System.Collections.Generic;

namespace NanoBuilder
{
   internal class ConstructorMatcher
   {
      public ConstructorMatcher( IEnumerable<IConstructor> constructors )
      {
         if ( constructors == null )
         {
            throw new ArgumentException( "Constructors parameter must not be null", nameof( constructors ) );
         }
      }
   }
}
