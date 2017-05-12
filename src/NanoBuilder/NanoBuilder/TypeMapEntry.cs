namespace NanoBuilder
{
   internal class TypeMapEntry
   {
      public object Instance
      {
         get;
      }

      public bool HasBeenMapped
      {
         get;
         set;
      }

      public TypeMapEntry( object instance )
      {
         Instance = instance;
      }
   }
}
