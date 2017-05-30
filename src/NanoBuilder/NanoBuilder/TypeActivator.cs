namespace NanoBuilder
{
   internal class TypeActivator : ITypeActivator
   {
      public T Default<T>()
      {
         return default( T );
      }
   }
}
