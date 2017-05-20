namespace NanoBuilder
{
   internal static class Resources
   {
      private const string _ambiguousConstructorMessage = "Multiple constructors were found matching the mapped types. Be more specific by mapping more types.\r\n\r\nConstructors found:\r\n{0}";
      public static string AmbiguousConstructorMessage => _ambiguousConstructorMessage;

      private const string _mapperMessage = "The interface mapper can only be set once. Did you call MapInterfacesTo() more than once?";
      public static string MapperMessage = _mapperMessage;

      private const string _moqMapperMessage = "Unable to locate Moq.Mock type. Are you referencing Moq?";
      public static string MoqMapperMessage = _moqMapperMessage;

      private const string _nsubstituteMapperMessage = "Unable to locate NSubstitute.Substitute type. Are you referencing NSubstitute?";
      public static string NSubstituteMapperMessage = _nsubstituteMapperMessage;

      private const string _parameterMappingMessage = "No public constructors on type {0} were found to accept a parameter of type {1}";
      public static string ParameterMappingMessage = _parameterMappingMessage;

      private const string _rhinoMocksMapperMessage = "Unable to locate the Rhino.Mocks.MockRepository type. Are you referencing RhinoMocks?";
      public static string RhinoMocksMapperMessage = _rhinoMocksMapperMessage;
   }
}
