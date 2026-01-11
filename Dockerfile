# syntax=docker/dockerfile:1.5

################################
# Build stage
################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia solution
COPY src/*.sln .

# Copia csproj (cache de restore)
COPY src/CarRental.Automotor.API/CarRental.Automotor.API.csproj CarRental.Automotor.API/
COPY src/CarRental.Automotor.Application/CarRental.Automotor.Application.csproj CarRental.Automotor.Application/
COPY src/CarRental.Automotor.Domain/CarRental.Automotor.Domain.csproj CarRental.Automotor.Domain/
COPY src/CarRental.Automotor.Infrastructure/CarRental.Automotor.Infrastructure.csproj CarRental.Automotor.Infrastructure/

# Restore
RUN dotnet restore

# Copia todo o código
COPY src/. .

# Publish
WORKDIR /src/CarRental.Automotor.API
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

################################
# Runtime stage
################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "CarRental.Automotor.API.dll"]