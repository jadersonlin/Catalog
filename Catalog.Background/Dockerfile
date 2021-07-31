#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Catalog.Background/Catalog.Background.csproj", "Catalog.Background/"]
COPY ["CatalogoAPI.Infrastructure/Catalog.Infrastructure.csproj", "CatalogoAPI.Infrastructure/"]
COPY ["CatalogoAPI.Application/Catalog.Application.csproj", "CatalogoAPI.Application/"]
COPY ["CatalogoAPI.Domain/Catalog.Domain.csproj", "CatalogoAPI.Domain/"]
RUN dotnet restore "Catalog.Background/Catalog.Background.csproj"
COPY . .
WORKDIR "/src/Catalog.Background"
RUN dotnet build "Catalog.Background.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.Background.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Background.dll"]
