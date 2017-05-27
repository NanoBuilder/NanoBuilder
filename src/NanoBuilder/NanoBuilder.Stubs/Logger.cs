namespace NanoBuilder.Stubs
{
   public class Logger
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
