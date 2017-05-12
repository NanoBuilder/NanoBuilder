namespace NanoBuilder.Tests.Stubs
{
   public interface IFileSystem
   {
   }

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
