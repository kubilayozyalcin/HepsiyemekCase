#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Services/HepsiYemek.Products/HepsiYemek.Products.csproj", "Services/HepsiYemek.Products/"]
COPY ["Core/HepsiYemek.Core/HepsiYemek.Core.csproj", "Core/HepsiYemek.Core/"]
RUN dotnet restore "Services/HepsiYemek.Products/HepsiYemek.Products.csproj"
COPY . .
WORKDIR "/src/Services/HepsiYemek.Products"
RUN dotnet build "HepsiYemek.Products.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HepsiYemek.Products.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HepsiYemek.Products.dll"]
