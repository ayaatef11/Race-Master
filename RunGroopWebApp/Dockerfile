# Base runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image with SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["RunGroopWebApp/RunGroopWebApp.csproj", "RunGroopWebApp/"]

# Restore dependencies
RUN dotnet restore "RunGroopWebApp/RunGroopWebApp.csproj"

# Copy entire source and build the application
COPY . .
WORKDIR "/src/RunGroopWebApp"
RUN dotnet build "RunGroopWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RunGroopWebApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RunGroopWebApp.dll"]
