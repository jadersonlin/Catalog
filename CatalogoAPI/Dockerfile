#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CatalogoAPI/Catalog.API.csproj", "CatalogoAPI/"]
COPY ["CatalogoAPI.Infrastructure/Catalog.Infrastructure.csproj", "CatalogoAPI.Infrastructure/"]
COPY ["CatalogoAPI.Application/Catalog.Application.csproj", "CatalogoAPI.Application/"]
COPY ["CatalogoAPI.Domain/Catalog.Domain.csproj", "CatalogoAPI.Domain/"]
RUN dotnet restore "CatalogoAPI/Catalog.API.csproj"
COPY . .
WORKDIR "/src/CatalogoAPI"
RUN dotnet build "Catalog.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.API.dll"]
