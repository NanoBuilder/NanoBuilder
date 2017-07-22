using System;
using NanoBuilder.Flow;

namespace NanoBuilder
{
   /// <summary>
   /// Represents an error when the given constructor parameter configuration matches multiple
   /// constructors. In this case, the <see cref="FullParameterComposer{T}" /> doesn't know what constructor
   /// to use, since there are multiple options.
   /// </summary>
   public class AmbiguousConstructorException : Exception
   {
      /// <summary>
      /// Initializes a new instance of the AmbiguousConstructorException class.
      /// </summary>
      public AmbiguousConstructorException()
      {
      }

      /// <summary>
      /// Initializes a new instance of the AmbiguousConstructorException class with a specified error message.
      /// </summary>
      /// <param name="message">The message that describes the error.</param>
      public AmbiguousConstructorException( string message )
         : base( message )
      {
      }

      /// <summary>
      /// Initializes a new instance of the AmbiguousConstructorException class with a specified error
      /// message and a reference to the inner exception that is the cause of this exception.
      /// </summary>
      /// <param name="message">The error message that explains the reason for the exception.</param>
      /// <param name="innerException">
      /// The exception that is the cause of the current exception, or a null reference
      /// (Nothing in Visual Basic) if no inner exception is specified.</param>
      public AmbiguousConstructorException( string message, Exception innerException )
         : base( message, innerException )
      {
      }
   }
}
