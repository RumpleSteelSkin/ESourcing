FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ESourcing.UI/ESourcing.UI.csproj", "ESourcing.UI/"]
COPY ["ESourcing.Core/ESourcing.Core.csproj", "ESourcing.Core/"]
COPY ["ESourcing.Infrastructure/ESourcing.Infrastructure.csproj", "ESourcing.Infrastructure/"]
RUN dotnet restore "ESourcing.UI/ESourcing.UI.csproj"
COPY . .
WORKDIR "/src/ESourcing.UI"
RUN dotnet build "./ESourcing.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ESourcing.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESourcing.UI.dll"]
