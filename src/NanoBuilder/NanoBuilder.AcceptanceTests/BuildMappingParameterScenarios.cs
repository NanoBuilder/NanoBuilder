﻿using System;
using System.ComponentModel;
using Xunit;
using FluentAssertions;
using Moq;
using NanoBuilder.Stubs;

namespace NanoBuilder.AcceptanceTests
{
   public class BuildMappingParameterScenarios
   {
      [Fact]
      public void MappingFirstParameterButNotTheSecondWillUseItsDefaultValueForTheSecond()
      {
         var eventArgs = ObjectBuilder.For<ProgressChangedEventArgs>()
            .With( 80 )
            .Build();

         eventArgs.UserState.Should().Be( default( object ) );
      }

      [Fact]
      public void MappingFirstParameterWillSetTheFirstParameter()
      {
         var innerException = new OverflowException();

         var exception = ObjectBuilder.For<Exception>()
            .With<Exception>( innerException )
            .Build();

         exception.InnerException.Should().Be( innerException );
      }

      [Fact]
      public void MappingOnlyTheFirstParameterWillActuallySetTheFirstParameter()
      {
         const int value = 5;

         var vertex = ObjectBuilder.For<Vertex>()
            .With( value )
            .Build();

         vertex.X.Should().Be( value );
         vertex.Y.Should().Be( default( int ) );
      }

      [Fact]
      public void MappingSingleParameterAsMockObjectWillSetCorrectly()
      {
         var fileSystemMock = new Mock<IFileSystem>();

         var logger = ObjectBuilder.For<Logger>()
            .With( fileSystemMock.Object )
            .Build();

         logger.FileSystem.Should().Be( fileSystemMock.Object );
      }

      [Fact]
      public void PassingIdenticalParameterTypesWillBeUsedInTheCorrectOrder()
      {
         const int year = 2017;
         const int month = 5;
         const int day = 15;

         var dateTime = ObjectBuilder.For<DateTime>()
            .With( year )
            .With( month )
            .With( day )
            .Build();

         dateTime.Year.Should().Be( year );
         dateTime.Month.Should().Be( month );
         dateTime.Day.Should().Be( day );
      }
   }
}
