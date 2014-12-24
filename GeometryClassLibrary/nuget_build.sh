cd "GeometryClassLibrary"
rm *.nupkg
STAMP="$(date +%s)"
nuget pack GeometryClassLibrary.csproj -Version 1.1.$STAMP
nuget push *.nupkg -ApiKey b669e128-f9fc-4af8-8594-3b35cb1a11cc