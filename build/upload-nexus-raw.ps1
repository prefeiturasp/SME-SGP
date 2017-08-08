[CmdletBinding()]
Param(
    [string]$NexusUrl,    
    [string]$User,
    [string]$Password,
    [string]$ProjectRepository
)

$pair = "$($user):$($password)";
$encodedCreds = [System.Convert]::ToBase64String([System.Text.Encoding]::ASCII.GetBytes($pair));
$basicAuthValue = "Basic $encodedCreds"

$Headers = @{
    Authorization = $basicAuthValue
}

$fileToUpload = (ls .\output\*zip)
$uploadUrl="$nexusUrl/repository/$projectRepository/$($fileToUpload.Name)"

Invoke-RestMethod -Uri $uploadUrl -Headers $Headers -Method Put -InFile $fileToUpload.FullName

