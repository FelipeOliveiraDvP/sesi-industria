FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY apps/backend/src/Catalog.Api/Catalog.Api.csproj ./Catalog.Api/
RUN dotnet restore ./Catalog.Api/Catalog.Api.csproj

COPY apps/backend/src/Catalog.Api/ ./Catalog.Api/
WORKDIR /source/Catalog.Api
RUN dotnet publish Catalog.Api.csproj \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    -p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish/ ./

ENV ASPNETCORE_ENVIRONMENT=Production \
    ASPNETCORE_HTTP_PORTS=8080 \
    PORT=8080

EXPOSE 8080
USER app
CMD ["sh", "-c", "exec dotnet Catalog.Api.dll --urls http://0.0.0.0:${PORT:-8080}"]
