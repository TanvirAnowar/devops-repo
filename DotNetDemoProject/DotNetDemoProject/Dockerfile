#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# 1. Build
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
# 1.1 restore
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["DotNetDemoProject.csproj", "."]
RUN dotnet restore "./DotNetDemoProject.csproj"

#1.2 build
COPY . .
WORKDIR "/src/."
RUN dotnet build "DotNetDemoProject.csproj" -c Release -o /app/build

# 2. publish
FROM build AS publish
RUN dotnet publish "DotNetDemoProject.csproj" -c Release -o /app/publish

# 3. Run
FROM base AS final
ENV ASPNETCORE_HTTP_PORT=5001
EXPOSE 5001:80
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNetDemoProject.dll"]