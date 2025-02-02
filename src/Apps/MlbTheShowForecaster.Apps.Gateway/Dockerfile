FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000

# .NET Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG BUILD_ID
ENV BUILD_ID=${BUILD_ID}
WORKDIR /src
# Build properties
COPY Directory.Build.props .
# Dependencies
COPY ["src/Apps/MlbTheShowForecaster.Apps.Gateway/MlbTheShowForecaster.Apps.Gateway.csproj", "."]
RUN dotnet restore "MlbTheShowForecaster.Apps.Gateway.csproj"
# Source code for building
COPY src/Apps/MlbTheShowForecaster.Apps.Gateway .

# If building for docker compose, replace the gateway routes with the service names
ARG IS_DOCKER_COMPOSE=false
RUN if [ "$IS_DOCKER_COMPOSE" = "true" ]; then \
    perl -0777 -pe 's/"Host": "localhost",\s*"Port": 5003/"Host": "mlb-the-show-forecaster-marketplace-watcher", "Port": 5003/g' -i ocelot.json && \
    perl -0777 -pe 's/"Host": "localhost",\s*"Port": 5002/"Host": "mlb-the-show-forecaster-performance-tracker", "Port": 5002/g' -i ocelot.json && \
    perl -0777 -pe 's/"Host": "localhost",\s*"Port": 5001/"Host": "mlb-the-show-forecaster-player-tracker", "Port": 5001/g' -i ocelot.json && \
    perl -0777 -pe 's/"DownstreamScheme": "https"/"DownstreamScheme": "http"/g' -i ocelot.json && \
    perl -0777 -pe 's/"BaseUrl":\s*"https:\/\/localhost:5000"/"BaseUrl": "http:\/\/mlb-the-show-forecaster-gateway:5000"/g' -i ocelot.json; \
    echo "ocelot.json:" && cat ocelot.json; \
else \
    echo "IS_DOCKER_COMPOSE=false, ocelot.json remains unchanged"; \
fi

# Build
RUN dotnet build "MlbTheShowForecaster.Apps.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

# .NET Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MlbTheShowForecaster.Apps.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# SPA asset stage - retrieves the required files to run the SPA
FROM alpine AS spa-assets
RUN apk add --no-cache curl unzip jq

# In the SPA's repo, get either the latest or specified release which will contain the SPA files
ARG REPO_OWNER=bretten
ARG REPO_NAME=mlb-the-show-forecaster-ui
ARG RELEASE_TAG
RUN echo "Using UI SPA release $RELEASE_TAG" # Invalidates Docker stage cache if the release tag changes
RUN if [ -z "$RELEASE_TAG" ]; then \
        # No specific release, so get the latest
        curl -s "https://api.github.com/repos/${REPO_OWNER}/${REPO_NAME}/releases/latest" \
            -o release.json; \
    else \
        # Get the specific release
        curl -s "https://api.github.com/repos/${REPO_OWNER}/${REPO_NAME}/releases/tags/${RELEASE_TAG}" \
            -o release.json; \
    fi

# Parse the release ID from the Github API response and use it to download the zip containing the SPA files
RUN mkdir -p /assets && \
    asset_id=$(jq -r '.assets[] | select(.name=="dist.zip") | .id' release.json) && \
    if [ -z "$asset_id" ] || [ "$asset_id" = "null" ]; then \
        echo "No dist.zip found" >&2; \
        exit 1; \
    fi && \
    echo "Asset ID: $asset_id" && \
    curl -L  -H "Accept: application/octet-stream" "https://api.github.com/repos/${REPO_OWNER}/${REPO_NAME}/releases/assets/${asset_id}" -o /assets/dist.zip && \
    ls -l /assets/dist.zip && \
    unzip /assets/dist.zip -d /assets && \
    rm /assets/dist.zip && rm release.json

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=spa-assets /assets ./wwwroot

# Allow the ocelot override file to be updated at startup (needs temp root access)
USER root
RUN echo '' > ocelot_override.json && \
    chown app:app ocelot_override.json && \
    chmod 644 ocelot_override.json
USER app

ENTRYPOINT ["dotnet", "MlbTheShowForecaster.Apps.Gateway.dll"]
