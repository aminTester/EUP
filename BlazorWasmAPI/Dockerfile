# Use the official .NET 8 SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore dependencies
COPY . ./
RUN dotnet restore

# Publish the project to the 'out' folder
RUN dotnet publish -c Release -o /out

# Use the official .NET runtime to run the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy the published files
COPY --from=build /out ./

# Expose the port (same as in appsettings.json)
EXPOSE 5000
EXPOSE 5001

# Start the application
ENTRYPOINT ["dotnet", "BlazorWasmAPI.dll"]
