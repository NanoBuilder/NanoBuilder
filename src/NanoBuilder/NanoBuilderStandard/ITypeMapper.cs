using System;

namespace NanoBuilder
{
   /// <summary>
   /// Represents an object that can map one kind of Type to another.
   /// <remarks>
   /// This is used by the <see cref="FullParameterComposer{T}"/> class when transforming constructor
   /// parameter types into other types. For example, mapping all interfaces into mock versions of
   /// the interface.
   /// </remarks>
   /// </summary>
   public interface ITypeMapper
   {
      /// <summary>
      /// Creates an instance that maps to the specific <see cref="Type"/>.
      /// </summary>
      /// <param name="type">The <see cref="Type"/> of object that needs to be mapped.</param>
      /// <returns>An object that represents the mapped type.</returns>
      object CreateForInterface( Type type );
   }
}
