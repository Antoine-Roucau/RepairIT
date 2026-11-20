@echo off
cd /d "%~dp0"

dotnet build RepairIT.slnx || (echo Echec de la compilation & pause & exit /b 1)

start "AuthentificationService" dotnet run --no-build --project src\RepairIT.AuthentificationService --launch-profile http
start "UserService"             dotnet run --no-build --project src\RepairIT.UserService --launch-profile http
start "CatalogService"          dotnet run --no-build --project src\RepairIT.CatalogService --launch-profile http
start "CustomerService"         dotnet run --no-build --project src\RepairIT.CustomerService --launch-profile http
start "RepairService"           dotnet run --no-build --project src\RepairIT.RepairService --launch-profile http
start "SalesService"            dotnet run --no-build --project src\RepairIT.SalesService --launch-profile http
start "Gateway"                 dotnet run --no-build --project src\RepairIT.Gateway --launch-profile http
start "Client"                  dotnet run --no-build --project src\RepairIT.Client --launch-profile http

echo Tous les projets sont lances. Front : http://localhost:5000