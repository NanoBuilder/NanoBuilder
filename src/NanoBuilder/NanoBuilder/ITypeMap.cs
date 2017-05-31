using System;

namespace NanoBuilder
{
   internal interface ITypeMap
   {
      void Add<TParameterType>( TParameterType parameter );

      Type[] Flatten();

      (object, bool) Get( Type parameterType );
   }
}