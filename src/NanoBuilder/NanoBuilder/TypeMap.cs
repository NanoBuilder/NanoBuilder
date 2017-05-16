using System;
using System.Collections.Generic;

namespace NanoBuilder
{
   internal class TypeMap
   {
      private readonly Dictionary<Type, Queue<object>> _mappings = new Dictionary<Type, Queue<object>>();

      public void Add<TParameterType>( TParameterType parameter )
      {
         var parameterType = typeof( TParameterType );

         if ( !_mappings.TryGetValue( parameterType, out var typeMapEntries ) )
         {
            typeMapEntries = new Queue<object>();
            _mappings[parameterType] = typeMapEntries;
         }

         typeMapEntries.Enqueue( parameter );
      }

      public (object, bool) Get( Type parameterType )
      {
         if ( _mappings.TryGetValue( parameterType, out var queue ) )
         {
            if ( queue.Count > 0 )
            {
               return (queue.Dequeue(), true);
            }
         }

         return (null, false);
      }
   }
}