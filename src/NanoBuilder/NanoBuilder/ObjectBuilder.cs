﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   /// <summary>
   /// Provides a way of creating objects by specifying arguments for the constructor. Relevant arguments
   /// are mapped and used to create the object; unmapped arguments are initialized to their defaults unless
   /// otherwise configured.
   /// </summary>
   public static class ObjectBuilder
   {
      /// <summary>
      /// Begins building an object of the specified type.
      /// </summary>
      /// <typeparam name="T">The type of object to build.</typeparam>
      /// <returns>An ObjectBuilder instance that can build the given type.</returns>
      public static IObjectBuilder<T> For<T>() => new ObjectBuilder<T>( new TypeInspector() );
   }

   internal class ObjectBuilder<T> : IObjectBuilder<T>
   {
      private readonly TypeMap _typeMap = new TypeMap();
      private readonly ITypeInspector _typeInspector;
      private ITypeMapper _interfaceMapper;

      internal ObjectBuilder( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      public IObjectBuilder<T> MapInterfacesTo<TMapperType>() where TMapperType : ITypeMapper
      {
         var constructor = typeof( TMapperType ).GetConstructors( BindingFlags.NonPublic | BindingFlags.Instance ).Single();
         _interfaceMapper = (ITypeMapper) constructor.Invoke( new object[] { _typeInspector } );

         return this;
      }

      public IObjectBuilder<T> With<TParameterType>( Func<TParameterType> parameterProvider )
      {
         var instance = parameterProvider();

         _typeMap.Add( instance );

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

            var (parameter, found) = _typeMap.Get( constructorParameters[index].ParameterType );

            if ( found )
            {
               callingParameters[index] = parameter;
            }
         }

         return (T) constructor.Invoke( callingParameters );
      }

      private static ConstructorInfo MatchConstructor( ConstructorInfo[] constructors, TypeMap typeMap )
      {
         var indexedConstructors = new Dictionary<ConstructorInfo, int>();
         var mappedParameterTypes = typeMap.Flatten();

         foreach ( var constructor in constructors )
         {
            var parameterTypes = constructor.GetParameters().Select( p => p.ParameterType );

            if ( parameterTypes.SequenceEqual( mappedParameterTypes ) )
            {
               return constructor;
            }

            var intersection = parameterTypes.Common( mappedParameterTypes );

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
