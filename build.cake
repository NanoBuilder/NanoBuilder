#tool "nuget:?package=xunit.runner.console"

var target = Argument( "target", "Default" );
var configuration = Argument( "configuration", "Release" );

var buildDir = Directory( "./src/NanoBuilder/NanoBuilder/bin" ) + Directory( configuration );
var artifactsDir = Directory( "./artifacts" );

//===========================================================================
// Clean Task
//===========================================================================

Task( "Clean" )
   .Does( () =>
{
   CleanDirectory( buildDir );
   CleanDirectory( artifactsDir );
});

//===========================================================================
// Restore Task
//===========================================================================

Task( "RestoreNuGetPackages" )
   .IsDependentOn( "Clean" )
   .Does( () =>
{
   NuGetRestore( "./src/NanoBuilder/NanoBuilder.sln" );
} );

//===========================================================================
// Build Task
//===========================================================================

Task( "Build" )
   .IsDependentOn( "RestoreNuGetPackages" )
   .Does( () =>
{
  MSBuild( "./src/NanoBuilder/NanoBuilder.sln", settings => settings.SetConfiguration( configuration ) );
} );

//===========================================================================
// Test Task
//===========================================================================

Task( "RunUnitTests" )
   .IsDependentOn( "Build" )
   .Does( () =>
{
   var testAssemblies = new[]
   {
      "./src/NanoBuilder/NanoBuilder.UnitTests/bin/" + Directory( configuration ) + "/NanoBuilder.UnitTests.dll",
      "./src/NanoBuilder/NanoBuilder.AcceptanceTests/bin/" + Directory( configuration ) + "/NanoBuilder.AcceptanceTests.dll",
   };

   XUnit2( testAssemblies );
} );

//===========================================================================
// Create NuGet Package Task
//===========================================================================

Task( "CreatePackage" )
   .IsDependentOn( "RunUnitTests" )
   .Does( () =>
{
   CreateDirectory( "./artifacts" );
   
   var settings = new NuGetPackSettings
   {   
     OutputDirectory = "./artifacts",
     ArgumentCustomization = args => args.Append( "-Prop Configuration=" + configuration )
   };
   
   NuGetPack( "./NanoBuilder.nuspec", settings );
} );

//===========================================================================
// Default Task
//===========================================================================

Task( "Default" )
   .IsDependentOn( "RunUnitTests" );

RunTarget( target );
