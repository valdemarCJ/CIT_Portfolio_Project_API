CIT GROUP 37 Portfolio 2 submission

Always run on https and port 7098 (https://localhost:7098/index.html):
run command: dotnet run --launch-profile https


integration tests:
dotnet test .\\test\\CIT_Portfolio_Project_API.IntegrationTests\\CIT_Portfolio_Project_API.IntegrationTests.csproj --no-restore --no-build

unit tests:
dotnet test .\test\CIT_Portfolio_Project_API.UnitTests\CIT_Portfolio_Project_API.UnitTests.csproj --no-restore --no-build




Shut down lingering build servers:
dotnet build-server shutdown


Clean, restore, and build (separately):
dotnet clean
dotnet restore
dotnet build -m

Timed the full sequence:
Measure-Command { dotnet clean; dotnet restore; dotnet build -m } | Select-Object -ExpandProperty TotalSeconds