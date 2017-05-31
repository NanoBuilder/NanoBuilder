namespace NanoBuilder
{
   internal interface ITypeActivator
   {
      ITypeMapper TypeMapper
      {
         get;
         set;
      }

      T Default<T>();
   }
}
