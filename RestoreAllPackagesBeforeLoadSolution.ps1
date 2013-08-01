$files = Get-ChildItem -Path .\src -Filter packages.config -Recurse 

foreach($f in $files)
{
    Write-Host ("" + $f.Directory + "\" + $f)
    .\.nuget\NuGet.exe install ("" + $f.Directory + "\" + $f) -solutionDir .\    
}