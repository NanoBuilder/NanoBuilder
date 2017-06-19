using System;
using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   /// <summary>
   /// Represents a mapper that can transform a type to an NSubstitute version of itself. The NSubstitute
   /// library must be present!
   /// </summary>
   public class NSubstituteMapper : ITypeMapper
   {
      private readonly ITypeInspector _typeInspector;

      internal NSubstituteMapper( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      /// <summary>
      /// Creates an NSubstitute mock that maps to the given type.
      /// </summary>
      /// <param name="type">The <see cref="Type"/> of object that needs to be mapped.</param>
      /// <returns>An object that represents the mapped type.</returns>
      /// <exception cref="TypeMapperException">
      /// Thrown if the NSubstitute Substitute type can't be found. This is likely from not referencing
      /// the NSubstitute library.
      /// </exception>
      public object CreateForInterface( Type type )
      {
         var mockType = _typeInspector.GetType( "NSubstitute.Substitute,NSubstitute" );

         if ( mockType == null )
         {
            throw new TypeMapperException( Resources.NSubstituteMapperMessage );
         }

         var forMethod = ( from m in mockType.GetTypeInfo().DeclaredMethods
                           from p in m.GetParameters()
                           where m.Name == "For"
                           && p.ParameterType == typeof( object[] )
                           && m.GetGenericArguments().Length == 1
                           select m ).Single();

         Type[] typeArguments = { type };
         var closedGenerateMethod = forMethod.MakeGenericMethod( typeArguments );

         object[] parameters = { new object[0] };
         return closedGenerateMethod.Invoke( null, parameters );
      }
   }
}
