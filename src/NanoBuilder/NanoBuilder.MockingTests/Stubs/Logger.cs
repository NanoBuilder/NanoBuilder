namespace NanoBuilder.MockingTests.Stubs
{
   internal class Logger
   {
      public IFileSystem FileSystem
      {
         get;
      }

      public Logger( IFileSystem fileSystem )
      {
         FileSystem = fileSystem;
      }
   }
}
