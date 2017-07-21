//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;

//namespace NanoBuilder
//{
//   internal class ConstructorMatcherOld : IConstructorMatcher
//   {
//      public ConstructorInfo Match( ConstructorInfo[] constructors, Type[] types )
//      {
//         var indexedConstructors = new Dictionary<ConstructorInfo, int>();

//         foreach ( var constructor in constructors )
//         {
//            var parameterTypes = constructor.GetParameters().Select( p => p.ParameterType );

//            if ( parameterTypes.SequenceEqual( types ) )
//            {
//               return constructor;
//            }

//            var intersection = parameterTypes.Common( types );

//            int sharedParameters = intersection.Count();
//            indexedConstructors.Add( constructor, sharedParameters );
//         }

//         var mostMatchedConstructors = indexedConstructors.OrderByDescending( k => k.Value );
//         int highestMatch = mostMatchedConstructors.First().Value;

//         var occurrencesWithHighestMatch = mostMatchedConstructors.Where( kvp => kvp.Value == highestMatch );
//         int overlappedMatches = occurrencesWithHighestMatch.Count();

//         if ( overlappedMatches > 1 )
//         {
//            string foundConstructorsMessage = occurrencesWithHighestMatch.Aggregate( string.Empty,
//               ( i, j ) => i + "  " + j.Key + Environment.NewLine );

//            string exceptionMessage = string.Format( Resources.AmbiguousConstructorMessage, foundConstructorsMessage );
//            throw new AmbiguousConstructorException( exceptionMessage );
//         }

//         return mostMatchedConstructors.First().Key;
//      }
//   }
//}