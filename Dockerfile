# syntax=docker/dockerfile:1.5

################################
# Build stage
################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solution
COPY src/*.sln .

# Copia csproj (cache de restore)
COPY src/Locadora.AutoMotors.API/Locadora.AutoMotors.API.csproj Locadora.AutoMotors.API/
COPY src/Locadora.AutoMotors.Application/Locadora.AutoMotors.Application.csproj Locadora.AutoMotors.Application/
COPY src/Locadora.AutoMotors.Domain/Locadora.AutoMotors.Domain.csproj Locadora.AutoMotors.Domain/
COPY src/Locadora.AutoMotors.Infrastructure/Locadora.AutoMotors.Infrastructure.csproj Locadora.AutoMotors.Infrastructure/

# Restore
RUN dotnet restore

# Copia todo o código
COPY src/. .

# Publish
WORKDIR /src/Locadora.AutoMotors.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

################################
# Runtime stage
################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Locadora.AutoMotors.API.dll"]