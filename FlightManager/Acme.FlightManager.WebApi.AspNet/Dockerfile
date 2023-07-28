#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["Acme.FlightManager.WebApi.AspNet/Acme.FlightManager.WebApi.AspNet.csproj", "Acme.FlightManager.WebApi.AspNet/"]
RUN dotnet restore "Acme.FlightManager.WebApi.AspNet/Acme.FlightManager.WebApi.AspNet.csproj"
COPY . .
WORKDIR "/src/Acme.FlightManager.WebApi.AspNet"
RUN dotnet build "Acme.FlightManager.WebApi.AspNet.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Acme.FlightManager.WebApi.AspNet.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Acme.FlightManager.WebApi.AspNet.dll"]