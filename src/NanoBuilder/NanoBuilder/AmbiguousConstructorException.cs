using System;
using System.Runtime.Serialization;

namespace NanoBuilder
{
   [Serializable]
   public class AmbiguousConstructorException : Exception
   {
      public AmbiguousConstructorException()
      {
      }

      public AmbiguousConstructorException( string message )
         : base( message )
      {
      }

      public AmbiguousConstructorException( string message, Exception inner )
         : base( message, inner )
      {
      }

      protected AmbiguousConstructorException( SerializationInfo info, StreamingContext context )
         : base( info, context )
      {
      }
   }
}
