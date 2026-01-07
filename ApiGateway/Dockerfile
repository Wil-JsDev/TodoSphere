# 1 Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY src/ApiGateway/*.csproj src/ApiGateway/
COPY src/Domain/*.csproj src/Domain/
COPY src/Infrastructure/*.csproj src/Infrastructure/

RUN dotnet restore src/ApiGateway/TodoSphere.ApiGateway.csproj

COPY src/ src/

RUN dotnet publish src/ApiGateway/TodoSphere.ApiGateway.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# 2 Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
USER app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "TodoSphere.ApiGateway.dll"]
