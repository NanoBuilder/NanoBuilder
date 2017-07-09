using System;
using System.Collections.Generic;
using System.Linq;

namespace NanoBuilder
{
   internal class ConstructorMatcher
   {
      private List<TypeEntry> _typeEntries = new List<TypeEntry>();
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
         if ( !_constructors.Any( c => c.ParameterTypes.Contains( typeof( T ) ) ) )
         {
            string message = string.Format( Resources.ParameterMappingMessage, "<Placeholder>", typeof( T ) );
            throw new ParameterMappingException( message );
         }

         var typeEntry = new TypeEntry( typeof( T ), instance );
         _typeEntries.Add( typeEntry );
      }

      public IEnumerable<IConstructor> GetMatches()
      {
         IEnumerable<IConstructor> matches = Enumerable.Empty<IConstructor>();

         var firstParameterMatches = _constructors.Where( c => c.ParameterTypes.First() == _typeEntries.First().Type );

         if ( firstParameterMatches.Count() > 1 )
         {
            throw new AmbiguousConstructorException();
         }

         if ( !_constructors.Any() )
         {
            return matches;
         }

         if ( _constructors.First().ParameterTypes.First() == _typeEntries.First().Type )
         {
            matches = new[] { _constructors.First() };
         }

         return matches;
      }
   }
}
