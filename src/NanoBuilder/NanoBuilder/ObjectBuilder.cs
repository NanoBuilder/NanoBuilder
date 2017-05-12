using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NanoBuilder
{
   public class ObjectBuilder<T>
   {
      private readonly Dictionary<Type, object> _typeMap = new Dictionary<Type, object>();

      private ObjectBuilder()
      {
      }

      public static ObjectBuilder<T> Create() => new ObjectBuilder<T>();

      public ObjectBuilder<T> With<TParameterType>( Expression<Func<TParameterType>> expr )
      {
         _typeMap[typeof( TParameterType )] = expr.Compile()();
         return this;
      }

      public T Build()
      {
         if ( typeof( T ) == typeof( string ) )
         {
            return default( T );
         }

         var constructors = typeof( T ).GetConstructors();

         if ( constructors.Length == 0 )
         {
            return default( T );
         }

         var constructor = MatchConstructor( constructors, _typeMap );
         var constructorParameters = constructor.GetParameters();

         var callingParameters = new object[constructorParameters.Length];

         for ( int index = 0; index < constructorParameters.Length; index++ )
         {
            if ( _typeMap.ContainsKey( constructorParameters[index].ParameterType ) )
            {
               callingParameters[index] = _typeMap[constructorParameters[index].ParameterType];
            }
         }

         return (T) constructor.Invoke( callingParameters );
      }

      private static ConstructorInfo MatchConstructor( ConstructorInfo[] constructors, Dictionary<Type, object> typeMap )
      {
         var indexedConstructors = new Dictionary<ConstructorInfo, int>();

         foreach ( var constructor in constructors )
         {
            var parameterTypes = constructor.GetParameters().Select( p => p.ParameterType );
            var intersection = parameterTypes.Intersect( typeMap.Keys );

            int sharedParameters = intersection.Count();
            indexedConstructors.Add( constructor, sharedParameters );
         }

         var mostMatchedConstructors = indexedConstructors.OrderByDescending( k => k.Value );
         int highestMatch = mostMatchedConstructors.First().Value;

         var occurrencesWithHighestMatch = mostMatchedConstructors.Where( kvp => kvp.Value == highestMatch );
         int overlappedMatches = occurrencesWithHighestMatch.Count();

         if ( overlappedMatches > 1 )
         {
            string foundConstructorsMessage = string.Empty;

            foreach ( var x in occurrencesWithHighestMatch )
            {
               foundConstructorsMessage += "  " + x.Key + Environment.NewLine;
            }

            string exceptionMessage = string.Format( Resources.AmbiguousConstructorMessage, foundConstructorsMessage );
            throw new AmbiguousConstructorException( exceptionMessage );
         }

         return mostMatchedConstructors.First().Key;
      }
   }
}
