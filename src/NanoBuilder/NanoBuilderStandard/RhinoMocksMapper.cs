using System;
using System.Linq;
using System.Reflection;

namespace NanoBuilder
{
   /// <summary>
   /// Represents a mapper that can transform a type to a RhinoMocks version of itself. The RhinoMocks
   /// library must be present!
   /// </summary>
   public class RhinoMocksMapper : ITypeMapper
   {
      private readonly ITypeInspector _typeInspector;

      internal RhinoMocksMapper( ITypeInspector typeInspector )
      {
         _typeInspector = typeInspector;
      }

      /// <summary>
      /// Creates a RhinoMocks mock that maps to the given type.
      /// </summary>
      /// <param name="type">The <see cref="Type"/> of object that needs to be mapped.</param>
      /// <returns>An object that represents the mapped type.</returns>
      /// <exception cref="TypeMapperException">
      /// Thrown if the RhinoMocks MockRepository type can't be found. This is likely from
      /// not referencing the RhinoMocks library.
      /// </exception>
      public object CreateForInterface( Type type )
      {
         var mockType = _typeInspector.GetType( "Rhino.Mocks.MockRepository,Rhino.Mocks" );

         if ( mockType == null )
         {
            throw new TypeMapperException( Resources.RhinoMocksMapperMessage );
         }

         var generateMethod = ( from m in mockType.GetTypeInfo().DeclaredMethods
                                from p in m.GetParameters()
                                where m.Name == "GenerateStub"
                                   && p.ParameterType == typeof( object[] )
                                   && m.GetGenericArguments().Length > 0
                                select m ).Single();

         Type[] typeArguments = { type };
         var closedGenerateMethod = generateMethod.MakeGenericMethod( typeArguments );

         object[] parameters = { new object[0] };
         return closedGenerateMethod.Invoke( null, parameters );
      }
   }
}
