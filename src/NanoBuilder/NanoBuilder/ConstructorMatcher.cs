using System;
using System.Collections.Generic;
using System.Linq;

namespace NanoBuilder
{
   internal class ConstructorMatcher
   {
      private readonly List<TypeEntry> _typeEntries = new List<TypeEntry>();
      private readonly IConstructor[] _constructors;

      public ConstructorMatcher( IEnumerable<IConstructor> constructors )
      {
         if ( constructors == null )
         {
            throw new ArgumentException( "Constructors parameter must not be null", nameof( constructors ) );
         }

         _constructors = constructors.ToArray();

         if ( _constructors.Length == 0 )
         {
            throw new ArgumentException( "Constructors parameter must not be null", nameof( constructors ) );
         }
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

      public IConstructor GetMatches()
      {
         var indexedConstructors = new Dictionary<IConstructor, int>();
         var types = _typeEntries.Select( t => t.Type );

         foreach ( var constructor in _constructors )
         {
            if ( constructor.ParameterTypes.SequenceEqual( types ) )
            {
               return constructor;
            }

            var intersection = constructor.ParameterTypes.Common( types );

            int sharedParameters = intersection.Count();
            indexedConstructors.Add( constructor, sharedParameters );
         }

         var mostMatchedConstructors = indexedConstructors.OrderByDescending( k => k.Value );
         int highestMatch = mostMatchedConstructors.First().Value;

         var occurrencesWithHighestMatch = mostMatchedConstructors.Where( kvp => kvp.Value == highestMatch );
         int overlappedMatches = occurrencesWithHighestMatch.Count();

         if ( overlappedMatches > 1 )
         {
            string foundConstructorsMessage = occurrencesWithHighestMatch.Aggregate( string.Empty,
                                                                                     ( i, j ) => i + "  " + j.Key + Environment.NewLine );

            string exceptionMessage = string.Format( Resources.AmbiguousConstructorMessage, foundConstructorsMessage );
            throw new AmbiguousConstructorException( exceptionMessage );
         }

         return mostMatchedConstructors.First().Key;
      }
   }
}
