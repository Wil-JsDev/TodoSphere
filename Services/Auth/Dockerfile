# 1 Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Services/Auth/TodoSphere.Auth.API/*.csproj ./Services/Auth/TodoSphere.Auth.API/
COPY Services/Auth/TodoSphere.Auth.Application/*.csproj ./Services/Auth/TodoSphere.Auth.Application/
COPY Services/Auth/TodoSphere.Auth.Domain/*.csproj ./Services/Auth/TodoSphere.Auth.Domain/
COPY Services/Auth/TodoSphere.Auth.Infrastructure/*.csproj ./Services/Auth/TodoSphere.Auth.Infrastructure/
COPY Shared/Shared/*.csproj ./Shared/Shared/

RUN dotnet restore Services/Auth/TodoSphere.Auth.API/TodoSphere.Auth.API.csproj

## Copy everything else and build
COPY Services/Auth/ Services/Auth/
COPY Shared/Shared/ Shared/Shared/

# Publish the application
RUN dotnet publish Services/Auth/TodoSphere.Auth.API/TodoSphere.Auth.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore 

# 2 Runtime \
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

USER app

COPY --from=build /app/publish .

EXPOSE 8080
CMD ["dotnet", "TodoSphere.Auth.API.dll"]