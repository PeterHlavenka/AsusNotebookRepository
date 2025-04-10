# Fáze 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Zkopíruj NuGet.config, pokud ho máš
COPY NuGet.Config . 

# Zkopíruj zbytek projektu
COPY . .

# Obnovení NuGet balíčků
RUN dotnet restore

# Publikuj výstup
RUN dotnet publish Adwind.Rabbit.ImportViewer.csproj -c Release -o /app/out

# Fáze 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Adwind.Rabbit.ImportViewer.dll"]
