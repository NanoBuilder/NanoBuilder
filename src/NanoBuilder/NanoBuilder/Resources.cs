namespace NanoBuilder
{
   internal static class Resources
   {
      public const string AmbiguousConstructorMessage = "Multiple constructors were found matching the mapped types. Be more specific by mapping more types.\r\n\r\nConstructors found:\r\n{0}";

      public const string FakeItEasyMessage = "Unable to locate FakeItEasy.A type. Are you referencing FakeItEasy?";

      public const string MapperMessage = "The interface mapper can only be set once. Did you call MapInterfacesTo() more than once?";

      public const string MoqMapperMessage = "Unable to locate Moq.Mock type. Are you referencing Moq?";

      public const string NSubstituteMapperMessage = "Unable to locate NSubstitute.Substitute type. Are you referencing NSubstitute?";

      public const string ParameterMappingMessage = "No public constructors on type {0} were found to accept a parameter of type {1}";

      public const string RhinoMocksMapperMessage = "Unable to locate the Rhino.Mocks.MockRepository type. Are you referencing RhinoMocks?";
   }
}
