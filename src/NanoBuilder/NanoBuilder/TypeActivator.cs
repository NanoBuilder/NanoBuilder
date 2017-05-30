namespace NanoBuilder
{
   internal class TypeActivator : ITypeActivator
   {
      public ITypeMapper TypeMapper
      {
         get;
         set;
      }

      public T Default<T>()
      {
         var type = typeof( T );

         if ( TypeMapper != null && type.IsInterface )
         {
            return (T) TypeMapper.CreateForInterface( type );
         }

         return default( T );
      }
   }
}
