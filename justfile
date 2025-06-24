_:
  @just --list

build:
  dotnet publish ./TaskSchedulerManager.csproj -r win-x64 --sc true -o $USERPROFILE/.local/bin
