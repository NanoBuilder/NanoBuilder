using System;
using System.Runtime.Serialization;

namespace NanoBuilder
{
   [Serializable]
   public class TypeMapperException : Exception
   {
      public TypeMapperException()
      {
      }

      public TypeMapperException( string message )
         : base( message )
      {
      }

      public TypeMapperException( string message, Exception inner )
         : base( message, inner )
      {
      }

      protected TypeMapperException( SerializationInfo info, StreamingContext context )
         : base( info, context )
      {
      }
   }
}
