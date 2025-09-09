# Step 1: Use the official ASP.NET runtime image (for running the app later)
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

# Make ASP.NET listen on all network interfaces on port 80
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Step 2: Use the .NET SDK image (for building the app)
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies (this caches NuGet packages)
COPY ["DotNetDemoProject.csproj", "./"]
RUN dotnet restore "DotNetDemoProject.csproj"

# Copy the rest of the source code and build the project
COPY . .
RUN dotnet build "DotNetDemoProject.csproj" -c Release -o /app/build

# Step 3: Publish the app (creates smaller, optimized output)
FROM build AS publish
RUN dotnet publish "DotNetDemoProject.csproj" -c Release -o /app/publish

# Step 4: Create the final image (only has runtime, not SDK)
FROM base AS final
WORKDIR /app

# Copy published files from the "publish" step
COPY --from=publish /app/publish .

# Run the app
ENTRYPOINT ["dotnet", "DotNetDemoProject.dll"]