﻿using System;
using System.Net;
using System.Net.Sockets;
using Xunit;
using FluentAssertions;

namespace NanoBuilder.AcceptanceTests
{
   public class VariedConstructorScenarios
   {
      [Fact]
      public void CanBuildObjectWithManyIntegers()
      {
         var version = ObjectBuilder.For<Version>()
            .With( 1 )
            .With( 2 )
            .With( 3 )
            .Build();

         version.ToString().Should().Be( "1.2.3" );
      }

      [Fact]
      public void CanBuildObjectWithManyIntegersAndSkipTheMiddle()
      {
         var version = ObjectBuilder.For<Version>()
            .With( 1 )
            .Skip<int>()
            .With( 3 )
            .Build();

         version.ToString().Should().Be( "1.0.3" );
      }

      [Fact]
      public void CanBuildObjectWithOnlyOneArgumentOmittingTheOther()
      {
         var socketAddress = ObjectBuilder.For<SocketAddress>()
            .With( 123 )
            .Build();
         
         socketAddress.Family.Should().Be( default( AddressFamily ) );
         socketAddress.Size.Should().Be( 123 );
      }

      [Fact]
      public void CanBuildObjectWithGenericParameter()
      {
         int[] array = { 1, 2, 3, 4, 5 };

         var arraySegment = ObjectBuilder.For<ArraySegment<int>>()
            .With( array )
            .With( 1 )
            .With( 3 )
            .Build();

         arraySegment.Array.Should().BeEquivalentTo( array );
         arraySegment.Offset.Should().Be( 1 );
         arraySegment.Count.Should().Be( 3 );
      }
   }
}
