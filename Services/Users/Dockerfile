# 1 Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Services/Users/TodoSphere.Users.API/TodoSphere.Users.API.csproj ./Services/Users/TodoSphere.Users.API/
COPY Services/Users/TodoSphere.Users.Application/TodoSphere.Users.Application.csproj ./Services/Users/TodoSphere.Users.Application/
COPY Services/Users/TodoSphere.Users.Domain/TodoSphere.Users.Domain.csproj ./Services/Users/TodoSphere.Users.Domain/
COPY Services/Users/TodoSphere.Users.Infrastructure/TodoSphere.Users.Infrastructure.csproj ./Services/Users/TodoSphere.Users.Infrastructure/

RUN dotnet restore Services/Users/TodoSphere.Users.API/TodoSphere.Users.API.csproj

## Copy everything else and build
COPY . .

# Publish the application
RUN dotnet publish Services/Users/TodoSphere.Users.API/TodoSphere.Users.API.csproj -c Release -o /app/publish

# 2 Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80
CMD ["dotnet", "TodoSphere.Users.API.dll"]
