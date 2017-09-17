#!/bin/bash

cd Deployer
rm bin/Debug/*.nupkg
dotnet restore
dotnet build
dotnet pack

#dotnet pack -o ../.nupkgs
cd ..
