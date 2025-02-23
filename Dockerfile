# Use .NET 8 SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy the API and Shared project files
COPY ["BlazorWasmAPI/BlazorWasmAPI.csproj", "BlazorWasmAPI/"]
COPY ["BlazorWasmShared/BlazorWasmShared.csproj", "BlazorWasmShared/"]

# Restore dependencies
RUN dotnet restore "BlazorWasmAPI/BlazorWasmAPI.csproj"

# Copy all files
COPY . .

# Build and publish the application
RUN dotnet publish "BlazorWasmAPI/BlazorWasmAPI.csproj" -c Release -o /out

# Use ASP.NET runtime for final image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /out .

# Expose API port
EXPOSE 8080

# Start the API
ENTRYPOINT ["dotnet", "BlazorWasmAPI.dll"]