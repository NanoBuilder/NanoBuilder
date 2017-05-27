namespace NanoBuilder.Stubs
{
   public class FileSystemAggregator
   {
      public IFileSystem FileSystem
      {
         get;
      }

      public IFileSystem FileSystem2
      {
         get;
      }

      public FileSystemAggregator( IFileSystem fileSystem, IFileSystem fileSystem2 )
      {
         FileSystem = fileSystem;
         FileSystem2 = fileSystem2;
      }
   }
}
