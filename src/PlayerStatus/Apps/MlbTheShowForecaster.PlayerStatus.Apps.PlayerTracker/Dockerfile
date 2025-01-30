FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG BUILD_ID
ENV BUILD_ID=${BUILD_ID}
WORKDIR /src
# Build properties
COPY Directory.Build.props .
# Dependencies
COPY ["src/PlayerStatus/Apps/MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker/MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.csproj", "PlayerStatus/Apps/MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker/"]
COPY ["src/Common/Execution/MlbTheShowForecaster.Common.Execution.Host/MlbTheShowForecaster.Common.Execution.Host.csproj", "Common/Execution/MlbTheShowForecaster.Common.Execution.Host/"]
COPY ["src/PlayerStatus/MlbTheShowForecaster.PlayerStatus.Infrastructure/MlbTheShowForecaster.PlayerStatus.Infrastructure.csproj", "PlayerStatus/MlbTheShowForecaster.PlayerStatus.Infrastructure/"]
COPY ["src/Common/MlbTheShowForecaster.Common.Infrastructure/MlbTheShowForecaster.Common.Infrastructure.csproj", "Common/MlbTheShowForecaster.Common.Infrastructure/"]
COPY ["src/Common/MlbTheShowForecaster.Common.Application/MlbTheShowForecaster.Common.Application.csproj", "Common/MlbTheShowForecaster.Common.Application/"]
COPY ["src/Common/MlbTheShowForecaster.Common.Domain/MlbTheShowForecaster.Common.Domain.csproj", "Common/MlbTheShowForecaster.Common.Domain/"]
COPY ["src/Common/MlbTheShowForecaster.Common/MlbTheShowForecaster.Common.csproj", "Common/MlbTheShowForecaster.Common/"]
COPY ["src/ExternalApis/MlbTheShowForecaster.ExternalApis.MlbApi/MlbTheShowForecaster.ExternalApis.MlbApi.csproj", "ExternalApis/MlbTheShowForecaster.ExternalApis.MlbApi/"]
COPY ["src/PlayerStatus/MlbTheShowForecaster.PlayerStatus.Application/MlbTheShowForecaster.PlayerStatus.Application.csproj", "PlayerStatus/MlbTheShowForecaster.PlayerStatus.Application/"]
COPY ["src/PlayerStatus/MlbTheShowForecaster.PlayerStatus.Domain/MlbTheShowForecaster.PlayerStatus.Domain.csproj", "PlayerStatus/MlbTheShowForecaster.PlayerStatus.Domain/"]
COPY ["src/ExternalApis/MlbTheShowForecaster.ExternalApis.MlbApi.Fakes/MlbTheShowForecaster.ExternalApis.MlbApi.Fakes.csproj", "ExternalApis/MlbTheShowForecaster.ExternalApis.MlbApi.Fakes/"]
RUN dotnet restore "PlayerStatus/Apps/MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker/MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.csproj"
COPY src .
WORKDIR "/src/PlayerStatus/Apps/MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker"
RUN dotnet build "MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MlbTheShowForecaster.PlayerStatus.Apps.PlayerTracker.dll"]
