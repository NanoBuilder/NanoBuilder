using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   public static class ObjectBuilder
   {
      public static ObjectBuilder<T> For<T>() => new ObjectBuilder<T>();
   }

   public class ObjectBuilder<T>
   {
      private readonly Dictionary<Type, TypeMapEntry> _typeMap = new Dictionary<Type, TypeMapEntry>();
      private ITypeMapper _interfaceMapper;

      internal ObjectBuilder()
      {
      }

      public ObjectBuilder<T> MapInterfacesTo<TMapperType>() where TMapperType : ITypeMapper, new()
      {
         _interfaceMapper = new TMapperType();

         return this;
      }

      public ObjectBuilder<T> With<TParameterType>( Func<TParameterType> parameterProvider )
      {
         var instance = parameterProvider();

         _typeMap[typeof( TParameterType )] = new TypeMapEntry( instance );

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
            if ( constructorParameters[index].ParameterType.IsInterface )
            {
               if ( _interfaceMapper != null )
               {
                  object instance = _interfaceMapper.CreateForInterface( constructorParameters[index].ParameterType );
                  callingParameters[index] = instance;
               }
            }

            if ( _typeMap.ContainsKey( constructorParameters[index].ParameterType ) )
            {
               if ( !_typeMap[constructorParameters[index].ParameterType].HasBeenMapped )
               {
                  _typeMap[constructorParameters[index].ParameterType].HasBeenMapped = true;
                  callingParameters[index] = _typeMap[constructorParameters[index].ParameterType].Instance;
               }
            }
         }

         return (T) constructor.Invoke( callingParameters );
      }

      private static ConstructorInfo MatchConstructor( ConstructorInfo[] constructors, Dictionary<Type, TypeMapEntry> typeMap )
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
