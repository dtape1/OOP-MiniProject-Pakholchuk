# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY src/CarRental.Domain/CarRental.Domain.csproj src/CarRental.Domain/
COPY src/CarRental.Application/CarRental.Application.csproj src/CarRental.Application/
COPY src/CarRental.Infrastructure/CarRental.Infrastructure.csproj src/CarRental.Infrastructure/
COPY src/CarRental.Console/CarRental.Console.csproj src/CarRental.Console/
RUN dotnet restore src/CarRental.Console/CarRental.Console.csproj

COPY src/ src/
RUN dotnet publish src/CarRental.Console/CarRental.Console.csproj -c Release -o /app

# Runtime stage — alpine (менший розмір)
FROM mcr.microsoft.com/dotnet/runtime:9.0-alpine AS runtime
WORKDIR /app

LABEL maintainer="Pakholchuk"
LABEL version="1.0.0"
LABEL description="Car Rental System - OOP Mini Project"

COPY --from=build /app ./
RUN mkdir -p /app/data

ENV DOTNET_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "CarRental.Console.dll"]