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
    var apiKey = "c81e175454b24450b7fa329462d67c77"; //EnvironmentVariable("apiKey");
    
    foreach (var package in GetFiles($"{artifactsDirectory}/*.nupkg"))
    {
        DotNetCoreNuGetPush(package.FullPath, 
            new DotNetCoreNuGetPushSettings {
                Source = "http://192.168.0.102:5555/v3/index.json",//https://www.nuget.org/api/v2/package
                ApiKey = apiKey,
                SkipDuplicate = true
            });
    }
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);