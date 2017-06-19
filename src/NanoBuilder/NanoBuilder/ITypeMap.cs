using System;

namespace NanoBuilder
{
   internal interface ITypeMap
   {
      void Add<TParameterType>( TParameterType parameter );

      Type[] Flatten();

      Parameter Get( Type parameterType );
   }
}