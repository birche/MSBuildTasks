cd .\Deployer
rm .\bin\Debug\*.nupkg
dotnet restore
dotnet build

dotnet pack -o ../.nupkgs
cd ..
