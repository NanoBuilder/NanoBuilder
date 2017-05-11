namespace NanoBuilder
{
   public class ObjectBuilder<T>
   {
      private ObjectBuilder()
      {
      }

      public static ObjectBuilder<T> Create() => new ObjectBuilder<T>();

      public T Build() => default( T );
   }
}
