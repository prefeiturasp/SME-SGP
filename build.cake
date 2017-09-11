#addin "nuget:http://nexus-ci.educacao.intranet:8081/repository/nuget-group/?package=Build.Tools&version=0.4.0"
#addin "nuget:https://www.nuget.org/api/v2?package=Newtonsoft.Json&version=9.0.1"
#tool "nuget:?package=GitVersion.CommandLine"
#load "./build/ms-helpers.cake"
#load "./build/ms-tasks.cake"

                                       


//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

BuildParameters parameters = new BuildParameters();

var buildSettings = BuildSettings.GetFromJsonFile(@"./build/buildsettings.json");


///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////

Setup(context =>
{
    Information("Running tasks...");
    Information("Definindo os parâmetros...");
    buildSettings.SemVer = GitVersionHelper.GetVersion(context);
    Information("Building version {0}", buildSettings.SemVer);
    parameters.ZipFileName = buildSettings.OutputDirectory +"/" + buildSettings.BuildName +"_" + buildSettings.SemVer +".zip";;
    parameters.IsLocalBuild = BuildSystem.IsLocalBuild;
    parameters.OutputArtifacts = parameters.ZipFileName;
    parameters.ServerRepositoryBuild = parameters.ServerRepository + "\\" + buildSettings.BuildName;
    parameters.IsPublishBuild = IsBuildTaggedOnJenkins() || IsReleaseBranchOnJenkins();
});

Teardown(context =>
{
    Information("Finished running tasks.");
});


//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
.IsDependentOn("Clean")
    .IsDependentOn("WebProject")  
    .IsDependentOn("Database") 
    .IsDependentOn("WindowsServices")         
    .IsDependentOn("Zip");


//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);