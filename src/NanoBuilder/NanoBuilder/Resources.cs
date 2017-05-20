namespace NanoBuilder
{
   internal static class Resources
   {
      private const string _ambiguousConstructorMessage = "Multiple constructors were found matching the mapped types. Be more specific by mapping more types.\r\n\r\nConstructors found:\r\n{0}";
      public static string AmbiguousConstructorMessage => _ambiguousConstructorMessage;

      private const string _mapperMessage = "The interface mapper can only be set once. Did you call MapInterfacesTo() more than once?";
      public static string MapperMessage = _mapperMessage;

      private const string _parameterMappingMessage = "No public constructors on type {0} were found to accept a parameter of type {1}";
      public static string ParameterMappingMessage = _parameterMappingMessage;

      private const string _typeMapperMessage = "Unable to locate Moq.Mock type. Are you referencing Moq?";
      public static string TypeMapperMessage = _typeMapperMessage;
   }
}
