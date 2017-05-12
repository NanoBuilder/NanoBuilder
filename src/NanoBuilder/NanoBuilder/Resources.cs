namespace NanoBuilder
{
   internal static class Resources
   {
      private const string _ambiguousConstructorMessage = "Multiple constructors were found matching the mapped types. Be more specific by mapping more types.\r\n\r\nConstructors found:\r\n{0}";
      public static string AmbiguousConstructorMessage => _ambiguousConstructorMessage;
   }
}
