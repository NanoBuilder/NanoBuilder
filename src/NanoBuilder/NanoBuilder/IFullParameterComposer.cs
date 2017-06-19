namespace NanoBuilder
{
   /// <summary>
   /// A class that can configure constructor parameters.
   /// </summary>
   /// <typeparam name="T">The type of object to build.</typeparam>
   public interface IFullParameterComposer<T> : IParameterComposer<T>, IMappedInterfaceConstraint<T>
   {
   }
}
