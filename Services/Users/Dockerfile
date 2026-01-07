# 1 Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Services/Users/TodoSphere.Users.API/*.csproj ./Services/Users/TodoSphere.Users.API/

COPY Services/Users/TodoSphere.Users.Application/*.csproj ./Services/Users/TodoSphere.Users.Application/

COPY Services/Users/TodoSphere.Users.Domain/*.csproj ./Services/Users/TodoSphere.Users.Domain/

COPY Services/Users/TodoSphere.Users.Infrastructure/*.csproj ./Services/Users/TodoSphere.Users.Infrastructure/

COPY Shared/Shared/*.csproj ./Shared/Shared/

RUN dotnet restore Services/Users/TodoSphere.Users.API/TodoSphere.Users.API.csproj

## Copy everything else and build
COPY Services/Users/ Services/Users/
COPY Shared/Shared/ Shared/Shared/

# Publish the application
RUN dotnet publish Services/Users/TodoSphere.Users.API/TodoSphere.Users.API.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# 2 Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

USER app

COPY --from=build /app/publish .

EXPOSE 8080
CMD ["dotnet", "TodoSphere.Users.API.dll"]
