rm *.nupkg
STAMP="$(date +%s)"
nuget pack GeometryClassLibrary.csproj -Version 1.1.$STAMP