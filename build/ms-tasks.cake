Task("Clean")
    .Does(() =>
{    
    CleanDirectories("./src/**/bin/" + configuration);
    var outputDir = buildSettings.OutputDirectory;

    if(DirectoryExists(outputDir))
    {       
        DeleteDirectory(outputDir,recursive:true);
    }  
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{

    NuGetRestore(buildSettings.SolutionFile);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild(buildSettings.SolutionFile, settings => 
    settings.SetConfiguration(configuration)
            .SetVerbosity(Verbosity.Minimal));
});

Task("WebProject")
    .IsDependentOn("Build")
    .Does(() =>
{
    PublishAllWebSites(buildSettings);
});

Task("Database")
    .IsDependentOn("Build")  
    .Does(() =>
{
    PublishAllDatabases(buildSettings);
});

Task("WindowsServices")
    .IsDependentOn("Build")
    .Does(() =>
{
    PublishAllWindowsServices(buildSettings);
});

Task("ReportingServices")
    .IsDependentOn("Build")
    .Does(() =>
{
    PublishAllReportingServices(buildSettings);
});


Task("Zip")
    .Does(() =>
{
    Zip(buildSettings.PublishDirRootVersion,  parameters.ZipFileName);
});



