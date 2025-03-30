FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
#init Openshift
LABEL io.k8s.display-name="WALLET_SERVICE" \
      io.k8s.description="Web api WALLET_SERVICE" \
      io.openshift.expose-services="8080:http"

EXPOSE 8080
ENV ASPNETCORE_URLS=http://*:8080
ENV TZ=America/Bogota
#end Openshift

# Esta fase se usa para compilar el proyecto de servicio
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WALLET_SERVICE.Api/WALLET_SERVICE.Api.csproj", "WALLET_SERVICE.Api/"]
COPY ["WALLET_SERVICE.Application/WALLET_SERVICE.Application.csproj", "WALLET_SERVICE.Application/"]
COPY ["WALLET_SERVICE.Domain/WALLET_SERVICE.Domain.csproj", "WALLET_SERVICE.Domain/"]
COPY ["WALLET_SERVICE.Logger/WALLET_SERVICE.Logger.csproj", "WALLET_SERVICE.Logger/"]
COPY ["WALLET_SERVICE.Infrastructure/WALLET_SERVICE.Infrastructure.csproj", "WALLET_SERVICE.Infrastructure/"]
RUN dotnet restore "./WALLET_SERVICE.Api/WALLET_SERVICE.Api.csproj"
COPY . .
WORKDIR "/src/WALLET_SERVICE.Api"
RUN dotnet build "./WALLET_SERVICE.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase se usa para publicar el proyecto de servicio que se copiará en la fase final.
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WALLET_SERVICE.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase se usa en producción o cuando se ejecuta desde VS en modo normal (valor predeterminado cuando no se usa la configuración de depuración)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WALLET_SERVICE.Api.dll"]