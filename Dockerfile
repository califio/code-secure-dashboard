FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build_api_deps
WORKDIR /src
COPY code-secure-api/code-secure-api/code-secure-api.csproj .
RUN dotnet restore "code-secure-api.csproj"

FROM build_api_deps AS build_api
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY code-secure-api/code-secure-api .
RUN dotnet build "code-secure-api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build_api AS publish_api
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "code-secure-api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM node:22 AS build_ui_deps
WORKDIR /app
COPY code-secure-ui/package.json .
COPY code-secure-ui/package-lock.json .
RUN npm ci

FROM build_ui_deps AS build_ui
WORKDIR /app
COPY code-secure-ui .
RUN npm run build --prod

FROM base AS final
WORKDIR /app
COPY --from=publish_api /app/publish .
COPY --from=build_ui /app/dist/code-secure-dashboard/browser wwwroot
ENTRYPOINT ["dotnet", "code-secure-api.dll"]
