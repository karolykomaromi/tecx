param($installPath, $toolsPath, $package, $project)

$buildProject = @([Microsoft.Build.Evaluation.ProjectCollection]::GlobalProjectCollection.GetLoadedProjects($project.FullName))[0]  

$file = $buildProject.Xml.Items | Where Include -eq "Resources.tt"
$propertyToEdit = $file.Metadata | Where Name -eq "DependentUpon"

if (!$propertyToEdit)
{
    $file.AddMetaData("DependentUpon", "Resources.resx") | Out-Null
}

$project.Save()