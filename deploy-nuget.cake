var target = Argument("target", "Push-Nuget-Package");
var configuration = Argument("configuration", "Release");
var artifactsDirectory = MakeAbsolute(Directory("./artifacts"));

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    foreach (var project in GetFiles("./src/**/*.csproj"))
    {
       CleanDirectory($"{project.GetDirectory().FullPath}/bin/{configuration}");
    }
    
    CleanDirectory(artifactsDirectory);
});

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreBuild("./Nebularium.sln", new DotNetCoreBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Create-Nuget-Package")
.IsDependentOn("Build")
.Does(() =>
{
    foreach (var project in GetFiles("./src/**/*.csproj"))
    {
        DotNetCorePack(
            project.GetDirectory().FullPath,
            new DotNetCorePackSettings()
            {
                Configuration = configuration,
                OutputDirectory = artifactsDirectory
            });
    }
});

Task("Push-Nuget-Package")
.IsDependentOn("Create-Nuget-Package")
.Does(() =>
{
    var apiKey = EnvironmentVariable("apiKey");
    
    foreach (var package in GetFiles($"{artifactsDirectory}/*.nupkg"))
    {
        DotNetCoreNuGetPush(package.FullPath, 
            new DotNetCoreNuGetPushSettings {
                Source = "https://api.nuget.org/v3/index.json",
                ApiKey = apiKey,
                SkipDuplicate = true
            });
    }
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);