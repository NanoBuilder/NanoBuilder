using System;
using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   /// <summary>
   /// Represents a mapper that can transform a type to a FakeItEasy version of itself. The FakeItEasy
   /// library must be present!
   /// </summary>
   public class FakeItEasyMapper : ITypeMapper
   {
      private readonly ITypeInspector _typeInspector;

      internal FakeItEasyMapper( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      /// <summary>
      /// Creates a FakeItEasy instance that maps to the given type.
      /// </summary>
      /// <param name="type">The <see cref="Type"/> of object that needs to be mapped.</param>
      /// <returns>An object that represents the mapped type.</returns>
      /// <exception cref="TypeMapperException">
      /// Thrown if the FakeItEasy.A type can't be found. This is likely from not referencing the
      /// FakeItEasy library.
      /// </exception>
      public object CreateForInterface( Type type )
      {
         var mockType = _typeInspector.GetType( "FakeItEasy.A,FakeItEasy" );

         if ( mockType == null )
         {
            throw new TypeMapperException( Resources.FakeItEasyMessage );
         }

         var fakeMethod = ( from m in mockType.GetMethods( BindingFlags.Public | BindingFlags.Static )
                            where m.Name == "Fake"
                               && m.GetParameters().Length == 0
                               && m.GetGenericArguments().Length == 1
                            select m ).Single();

         Type[] typeArguments = { type };
         var closedGenerateMethod = fakeMethod.MakeGenericMethod( typeArguments );

         return closedGenerateMethod.Invoke( null, null );
      }
   }
}
