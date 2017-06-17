using System;

namespace NanoBuilder
{
   /// <summary>
   /// Represents an error internal to NanoBuilder. If you see this, it's likely a bug in NanoBuilder.
   /// </summary>
   [Serializable]
   public class NanoBuilderException : Exception
   {
      /// <summary>
      /// Initializes a new instance of the NanoBuilderException class.
      /// </summary>
      public NanoBuilderException()
      {
      }

      /// <summary>
      /// Initializes a new instance of the NanoBuilderException class with a specified error message.
      /// </summary>
      /// <param name="message">The message that describes the error.</param>
      public NanoBuilderException( string message )
         : base( message )
      {
      }

      /// <summary>
      /// Initializes a new instance of the NanoBuilderException class with a specified error
      /// message and a reference to the inner exception that is the cause of this exception.
      /// </summary>
      /// <param name="message">The error message that explains the reason for the exception.</param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference
      /// (Nothing in Visual Basic) if no inner exception is specified.</param>
      public NanoBuilderException( string message, Exception innerException )
         : base( message, innerException )
      {
      }
   }
}
