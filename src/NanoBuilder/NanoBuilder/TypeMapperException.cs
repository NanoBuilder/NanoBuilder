using System;

namespace NanoBuilder
{
   /// <summary>
   /// Represents an error when the Type Mapper is unable to transform the source type into the
   /// destination object.
   /// </summary>
   public class TypeMapperException : Exception
   {
      /// <summary>
      /// Initializes a new instance of the TypeMapperException class.
      /// </summary>
      public TypeMapperException()
      {
      }

      /// <summary>
      /// Initializes a new instance of the TypeMapperException class with a specified error message.
      /// </summary>
      /// <param name="message">The message that describes the error.</param>
      public TypeMapperException( string message )
         : base( message )
      {
      }

      /// <summary>
      /// Initializes a new instance of the TypeMapperException class with a specified error
      /// message and a reference to the inner exception that is the cause of this exception.
      /// </summary>
      /// <param name="message">The error message that explains the reason for the exception.</param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference
      /// (Nothing in Visual Basic) if no inner exception is specified.</param>
      public TypeMapperException( string message, Exception innerException )
         : base( message, innerException )
      {
      }
   }
}
