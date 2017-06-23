using System;
using System.Collections.Generic;
using System.Linq;

namespace NanoBuilder
{
   internal class ConstructorMatcher
   {
      private readonly IEnumerable<IConstructor> _constructors;

      public ConstructorMatcher( IEnumerable<IConstructor> constructors )
      {
         if ( constructors == null )
         {
            throw new ArgumentException( "Constructors parameter must not be null", nameof( constructors ) );
         }

         _constructors = constructors;
      }

      public void Add<T>( T instance )
      {
         if ( !_constructors.Any( c => c.Type == typeof( T ) ) )
         {
            string message = string.Format( Resources.ParameterMappingMessage, "<Placeholder>", typeof( T ) );
            throw new ParameterMappingException( message );
         }

         throw new NotImplementedException();
      }

      public IEnumerable<IConstructor> GetMatches()
      {
         return Enumerable.Empty<IConstructor>();
      }
   }
}
