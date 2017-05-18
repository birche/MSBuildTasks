#!/bin/bash

cd Deployer
rm bin/Debug/*.nupkg
dotnet restore
dotnet build
dotnet pack
nuget push bin/Debug/*.nupkg f15c1542-6ae8-42ed-9977-450a93e55d38 -source https://www.myget.org/F/birche/api/v2/package
cd ..