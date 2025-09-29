# 1 Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY TodoSphere.sln ./
COPY ApiGateway/TodoSphere.ApiGateway/TodoSphere.ApiGateway.csproj ./ApiGateway/

WORKDIR /app/ApiGateway

RUN dotnet restore

# Copy everything else and build
COPY . .

# Publish the application

RUN dotnet publish ApiGateway/TodoSphere.ApiGateway/TodoSphere.ApiGateway.csproj -c Release -o /app/publish

# 2 Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "TodoSphere.ApiGateway.dll"]
