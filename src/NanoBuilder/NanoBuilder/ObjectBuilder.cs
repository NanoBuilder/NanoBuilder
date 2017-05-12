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
      public static ObjectBuilder<T> For<T>() => new ObjectBuilder<T>( new TypeInspector() );
   }

   /// <summary>
   /// A specific builder that can be configured to create the specified object type.
   /// </summary>
   /// <typeparam name="T">The type of object to build.</typeparam>
   public class ObjectBuilder<T>
   {
      private readonly Dictionary<Type, TypeMapEntry> _typeMap = new Dictionary<Type, TypeMapEntry>();
      private readonly ITypeInspector _typeInspector;
      private ITypeMapper _interfaceMapper;

      internal ObjectBuilder( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      /// <summary>
      /// Configures how interface types should be initialized by default. 
      /// </summary>
      /// <typeparam name="TMapperType"></typeparam>
      /// <returns></returns>
      public ObjectBuilder<T> MapInterfacesTo<TMapperType>() where TMapperType : ITypeMapper
      {
         var constructor = typeof( TMapperType ).GetConstructors( BindingFlags.NonPublic | BindingFlags.Instance ).Single();
         _interfaceMapper = (ITypeMapper) constructor.Invoke( new object[] { _typeInspector } );

         return this;
      }

      /// <summary>
      /// Configures a parameter for the object's constructor.
      /// </summary>
      /// <typeparam name="TParameterType">The type of object for the constructor.</typeparam>
      /// <param name="parameterProvider">A <see cref="Func{TResult}"/> that provides the instance for this parameter.</param>
      /// <returns>The same <see cref="ObjectBuilder{T}"/>.</returns>
      public ObjectBuilder<T> With<TParameterType>( Func<TParameterType> parameterProvider )
      {
         var instance = parameterProvider();

         _typeMap[typeof( TParameterType )] = new TypeMapEntry( instance );

         return this;
      }

      /// <summary>
      /// Creates the instance with the configured constructor parameters.
      /// </summary>
      /// <returns>The object instance.</returns>
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
            string foundConstructorsMessage = occurrencesWithHighestMatch.Aggregate( string.Empty,
               ( i, j ) => i + "  " + j.Key + Environment.NewLine );

            string exceptionMessage = string.Format( Resources.AmbiguousConstructorMessage, foundConstructorsMessage );
            throw new AmbiguousConstructorException( exceptionMessage );
         }

         return mostMatchedConstructors.First().Key;
      }
   }
}