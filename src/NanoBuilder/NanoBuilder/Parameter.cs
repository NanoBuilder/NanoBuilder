namespace NanoBuilder
{
   internal class Parameter
   {
      public object Instance
      {
         get;
      }

      public bool WasFound
      {
         get;
      }

      public Parameter( object instance, bool wasFound )
      {
         Instance = instance;
         WasFound = wasFound;
      }
   }
}
