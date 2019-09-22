 $sourceDir = "packages_for_push"
 
 if(!(Test-Path $sourceDir))
 {
        Write-Error "Nuget packages directory not found"
        return
 }

 cd $sourceDir
 
 $exe = "..\Nuget.exe"

 $list = ls *.nupkg -Recurse | foreach FullName | sort

 
 foreach ($item in $list) 
 {

       &$exe push $item -Source http://mercurial/nuget/nuget NielsenDefault
 }

 cd..  
 

Pause