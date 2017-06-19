namespace NanoBuilder
{
   internal interface IMapperFactory
   {
      T Create<T>() where T: ITypeMapper;
   }
}
