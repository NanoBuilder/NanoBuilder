namespace NanoBuilder
{
   internal static class Resources
   {
      private const string _ambiguousConstructorMessage = "Multiple constructors were found matching the mapped types. Be more specific by mapping more types.\r\n\r\nConstructors found:\r\n{0}";
      public static string AmbiguousConstructorMessage => _ambiguousConstructorMessage;

      private const string _typeMapperMessage = "Unable to locate Moq.Mock type. Are you referencing Moq?";
      public static string TypeMapperMessage = _typeMapperMessage;
   }
}
