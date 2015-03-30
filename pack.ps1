if (Test-Path .build\packages\)
{
	rmdir -Recurse -Force .build\packages
}
mkdir .build\packages\
cd Diversion
nuget pack -Build -p Configuration=Release -OutputDirectory ..\.build\packages\
cd ..\Diversion.CLI
nuget pack -Build -p Configuration=Release -Tool -OutputDirectory ..\.build\packages\
cd ..
$publish = Read-Host "Publish (Y/N)?"
if ($publish -eq "Y")
{
	dir .build\packages | foreach  { nuget.exe push .build\packages\$_ }
}