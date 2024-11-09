FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5003

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/GameCards/Apps/MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher/MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.csproj", "GameCards/Apps/MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher/"]
COPY ["src/Common/Execution/MlbTheShowForecaster.Common.Execution.Host/MlbTheShowForecaster.Common.Execution.Host.csproj", "Common/Execution/MlbTheShowForecaster.Common.Execution.Host/"]
COPY ["src/GameCards/MlbTheShowForecaster.GameCards.Infrastructure/MlbTheShowForecaster.GameCards.Infrastructure.csproj", "GameCards/MlbTheShowForecaster.GameCards.Infrastructure/"]
COPY ["src/Common/MlbTheShowForecaster.Common.Infrastructure/MlbTheShowForecaster.Common.Infrastructure.csproj", "Common/MlbTheShowForecaster.Common.Infrastructure/"]
COPY ["src/Common/MlbTheShowForecaster.Common.Application/MlbTheShowForecaster.Common.Application.csproj", "Common/MlbTheShowForecaster.Common.Application/"]
COPY ["src/Common/MlbTheShowForecaster.Common.Domain/MlbTheShowForecaster.Common.Domain.csproj", "Common/MlbTheShowForecaster.Common.Domain/"]
COPY ["src/Common/MlbTheShowForecaster.Common/MlbTheShowForecaster.Common.csproj", "Common/MlbTheShowForecaster.Common/"]
COPY ["src/DomainApis/MlbTheShowForecaster.DomainApis.PerformanceApi/MlbTheShowForecaster.DomainApis.PerformanceApi.csproj", "DomainApis/MlbTheShowForecaster.DomainApis.PerformanceApi/"]
COPY ["src/DomainApis/MlbTheShowForecaster.DomainApis.PlayerStatusApi/MlbTheShowForecaster.DomainApis.PlayerStatusApi.csproj", "DomainApis/MlbTheShowForecaster.DomainApis.PlayerStatusApi/"]
COPY ["src/ExternalApis/MlbTheShowForecaster.ExternalApis.MlbTheShowApi/MlbTheShowForecaster.ExternalApis.MlbTheShowApi.csproj", "ExternalApis/MlbTheShowForecaster.ExternalApis.MlbTheShowApi/"]
COPY ["src/GameCards/MlbTheShowForecaster.GameCards.Application/MlbTheShowForecaster.GameCards.Application.csproj", "GameCards/MlbTheShowForecaster.GameCards.Application/"]
COPY ["src/GameCards/MlbTheShowForecaster.GameCards.Domain/MlbTheShowForecaster.GameCards.Domain.csproj", "GameCards/MlbTheShowForecaster.GameCards.Domain/"]
COPY ["src/ExternalApis/MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes/MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes.csproj", "ExternalApis/MlbTheShowForecaster.ExternalApis.MlbTheShowApi.Fakes/"]
RUN dotnet restore "GameCards/Apps/MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher/MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.csproj"
COPY src .
WORKDIR "/src/GameCards/Apps/MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher"
RUN dotnet build "MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MlbTheShowForecaster.GameCards.Apps.MarketplaceWatcher.dll"]
