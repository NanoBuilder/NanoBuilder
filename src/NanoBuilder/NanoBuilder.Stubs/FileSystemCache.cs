namespace NanoBuilder.Stubs
{
   internal class FileSystemCache
   {
      public IFileSystem FileSystem
      {
         get;
      }

      public int CacheSize
      {
         get;
      }

      public FileSystemCache( IFileSystem fileSystem, int cacheSize )
      {
         FileSystem = fileSystem;
         CacheSize = cacheSize;
      }
   }
}
