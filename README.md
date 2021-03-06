﻿[![Build status](https://img.shields.io/appveyor/ci/nanobuilder/nanobuilder.svg?style=flat-square)](https://ci.appveyor.com/project/NanoBuilder/nanobuilder)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/NanoBuilder.svg?colorB=dd6600&style=flat-square)](https://www.nuget.org/packages/NanoBuilder/)

## NanoBuilder

NanoBuilder is a modest way to construct objects by letting you specify only relevant constructor parameters. Omit the rest and NanoBuilder will fill 'em in with defaults or mocks.

## What for?

NanoBuilder helps you write maintainable tests.

When tests directly create objects, they _depend_ on the constructors. When constructors change, the tests also need to change.

Instead, NanoBuilder allows you to specify only the dependencies that are relevant to your scenario, decoupling the test from the system's constructors.

## How does it work?

Specify the type of object to build (the system being tested), pass objects with the fluent syntax (these will become the constructor parameters), and then call `Build()`!

```csharp
using NanoBuilder;

[Fact]
public void ExampleTest()
{
    var system = ObjectBuilder.For<System>()
        .With( "String parameter" )
        .With( 123 )
        .Build();

    // system has been created using a constructor that needs a string and int.
    // If other constructor parameters exist, they receive default values.
    // This test only supplies what is relevant to this scenario.
}
```

NanoBuilder creates the system under test using parameters you specify. Omitted parameters use their default values.

## What about mocks?

If you use constructor injection to pass dependencies to your systems, you probably pass mocks to these objects in your automated tests. NanoBuilder can automatically pass mocks in place of interfaces:

```csharp
using NanoBuilder;

[Fact]
public void ExampleTest()
{
    var complexSystem = ObjectBuilder.For<ComplexSystem>()
        .MapInterfacesWith<MoqMapper>()
        .Build();

    // Any interfaces that complexSystem received via constructor have automatically
    // become mocks. In this case, Moq mocks are used, but you can specify other mocking libraries.
}
```

This really shines when you pass your own mocks to your system and allow unspecified interfaces to be automatically mocked for you:

```csharp
using Moq;
using NanoBuilder;

[Fact]
public void ExampleTest()
{
    // Mock setup applies here
    var dependencyMock = new Mock<IDependency>();

    var complexSystem = ObjectBuilder.For<ComplexSystem>()
        .MapInterfacesWith<MoqMapper>()
        .With( dependencyMock.Object )
        .Build();

    // Now, complexSystem was created with our specific mock that stages this test scenario.
    // Any other interfaces it would accept have been automatically mocked out.
}
```

## Can I use other mocking libraries?

Yes! It all comes from whatever type is passed to `MapInterfacesWith<>`. There are several built into NanoBuilder that indicate how interfaces are automatically mocked. It currently supports the following:

- Moq (`MoqMapper`)
- RhinoMocks (`RhinoMocksMapper`)
- NSubstitute (`NSubstituteMapper`)
- FakeItEasy (`FakeItEasyMapper`)

**Make sure you've installed your mocking library's NuGet package, or NanoBuilder will fail to mock your interfaces and you'll get an exception!**

