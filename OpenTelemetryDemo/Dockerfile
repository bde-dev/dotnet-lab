FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OpenTelemetryDemo/OpenTelemetryDemo.csproj", "OpenTelemetryDemo/"]
RUN dotnet restore "OpenTelemetryDemo/OpenTelemetryDemo.csproj"
COPY . .
WORKDIR "/src/OpenTelemetryDemo"
RUN dotnet build "OpenTelemetryDemo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OpenTelemetryDemo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OpenTelemetryDemo.dll"]
