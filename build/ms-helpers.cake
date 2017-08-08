public class BuildParameters
{
    public string ServerRepository { get ;set; }
    public bool IsLocalBuild { get ;set; }
    public string ZipFileName { get ;set; }
    public string OutputArtifacts { get ;set; }
    public string ServerRepositoryBuild { get ;set; }
    public bool IsPublishBuild { get ;set; }
}

public class GitVersionHelper
{
    public static string GetVersion(ICakeContext context)
    {
        var isLocalBuild = context.BuildSystem().IsLocalBuild;               
        if(!isLocalBuild)
        {
            GitVersion versionInfo =   context.GitVersion(new GitVersionSettings
            {     
                OutputType = GitVersionOutput.BuildServer,
                UpdateAssemblyInfo = true
            });
        }
        GitVersion assertedVersions = context.GitVersion(new GitVersionSettings
        {
            OutputType = GitVersionOutput.Json,       
        });
        return assertedVersions.LegacySemVerPadded;
    }
}

