FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG BUILD_ID
ENV BUILD_ID=${BUILD_ID}
WORKDIR /src
# Build properties
COPY Directory.Build.props .
# Dependencies
COPY ["src/Performance/Apps/MlbTheShowForecaster.Performance.Apps.PerformanceTracker/MlbTheShowForecaster.Performance.Apps.PerformanceTracker.csproj", "Performance/Apps/MlbTheShowForecaster.Performance.Apps.PerformanceTracker/"]
COPY ["src/Common/Execution/MlbTheShowForecaster.Common.Execution.Host/MlbTheShowForecaster.Common.Execution.Host.csproj", "Common/Execution/MlbTheShowForecaster.Common.Execution.Host/"]
COPY ["src/Performance/MlbTheShowForecaster.Performance.Infrastructure/MlbTheShowForecaster.Performance.Infrastructure.csproj", "Performance/MlbTheShowForecaster.Performance.Infrastructure/"]
COPY ["src/Common/MlbTheShowForecaster.Common.Infrastructure/MlbTheShowForecaster.Common.Infrastructure.csproj", "Common/MlbTheShowForecaster.Common.Infrastructure/"]
COPY ["src/Common/MlbTheShowForecaster.Common.Application/MlbTheShowForecaster.Common.Application.csproj", "Common/MlbTheShowForecaster.Common.Application/"]
COPY ["src/Common/MlbTheShowForecaster.Common.Domain/MlbTheShowForecaster.Common.Domain.csproj", "Common/MlbTheShowForecaster.Common.Domain/"]
COPY ["src/Common/MlbTheShowForecaster.Common/MlbTheShowForecaster.Common.csproj", "Common/MlbTheShowForecaster.Common/"]
COPY ["src/ExternalApis/MlbTheShowForecaster.ExternalApis.MlbApi/MlbTheShowForecaster.ExternalApis.MlbApi.csproj", "ExternalApis/MlbTheShowForecaster.ExternalApis.MlbApi/"]
COPY ["src/Performance/MlbTheShowForecaster.Performance.Application/MlbTheShowForecaster.Performance.Application.csproj", "Performance/MlbTheShowForecaster.Performance.Application/"]
COPY ["src/Performance/MlbTheShowForecaster.Performance.Domain/MlbTheShowForecaster.Performance.Domain.csproj", "Performance/MlbTheShowForecaster.Performance.Domain/"]
COPY ["src/ExternalApis/MlbTheShowForecaster.ExternalApis.MlbApi.Fakes/MlbTheShowForecaster.ExternalApis.MlbApi.Fakes.csproj", "ExternalApis/MlbTheShowForecaster.ExternalApis.MlbApi.Fakes/"]
RUN dotnet restore "Performance/Apps/MlbTheShowForecaster.Performance.Apps.PerformanceTracker/MlbTheShowForecaster.Performance.Apps.PerformanceTracker.csproj"
COPY src .
WORKDIR "/src/Performance/Apps/MlbTheShowForecaster.Performance.Apps.PerformanceTracker"
RUN dotnet build "MlbTheShowForecaster.Performance.Apps.PerformanceTracker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MlbTheShowForecaster.Performance.Apps.PerformanceTracker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MlbTheShowForecaster.Performance.Apps.PerformanceTracker.dll"]
