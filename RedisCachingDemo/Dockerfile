FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RedisCachingDemo/RedisCachingDemo.csproj", "RedisCachingDemo/"]
COPY ["RepositoryPatternEntityFramework/RepositoryPatternEntityFramework.csproj", "RepositoryPatternEntityFramework/"]
RUN dotnet restore "RedisCachingDemo/RedisCachingDemo.csproj"
COPY . .
WORKDIR "/src/RedisCachingDemo"
RUN dotnet build "RedisCachingDemo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "RedisCachingDemo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RedisCachingDemo.dll"]
