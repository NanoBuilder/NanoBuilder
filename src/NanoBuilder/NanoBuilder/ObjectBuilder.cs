using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
         var constructors = typeof( T ).GetConstructors();

         if ( constructors.Length == 0 )
         {
            return default( T );
         }

         var constructor = constructors[0];
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
   }
}
