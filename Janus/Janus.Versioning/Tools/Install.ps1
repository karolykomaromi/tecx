param($installPath, $toolsPath, $package, $project)

$buildProject = @([Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName))[0]  

$globalAssemblyInfo = $buildProject.Xml.Items | Where Include -match "GlobalAssemblyInfo.cs"

if (!$globalAssemblyInfo)
{
    $metaData = New-Object 'system.collections.generic.dictionary[string,string]'
    $metaData.Add("Link", "Properties\GlobalAssemblyInfo.cs")

    $buildProject.AddItem("Compile", "..\Common\GlobalAssemblyInfo.cs", $metaData)
}

$project.Save()