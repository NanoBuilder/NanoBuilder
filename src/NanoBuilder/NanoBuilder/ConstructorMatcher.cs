using System;
using System.Collections.Generic;
using System.Linq;

namespace NanoBuilder
{
   internal class ConstructorMatcher
   {
      private List<TypeEntry> _typeEntries = new List<TypeEntry>();
      private readonly IEnumerable<IConstructor> _constructors;
      private IEnumerable<IConstructor> _currentMatches = Enumerable.Empty<IConstructor>();

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

         var typeEntry = new TypeEntry( typeof( T ), instance );
         _typeEntries.Add( typeEntry );

         UpdateMatches();
      }

      private void UpdateMatches()
      {
         if ( _constructors.First().Type == _typeEntries.First().Type )
         {
            _currentMatches = new [] { _constructors.First() };
         }
      }

      public IEnumerable<IConstructor> GetMatches() => _currentMatches;
   }
}
