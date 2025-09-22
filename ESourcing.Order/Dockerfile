FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ESourcing.Order/ESourcing.Order.csproj", "ESourcing.Order/"]
COPY ["ESourcing.Order.Application/ESourcing.Order.Application.csproj", "ESourcing.Order.Application/"]
COPY ["ESourcing.Order.Domain/ESourcing.Order.Domain.csproj", "ESourcing.Order.Domain/"]
COPY ["ESourcing.Order.Infrastructure/ESourcing.Order.Infrastructure.csproj", "ESourcing.Order.Infrastructure/"]
COPY ["EventBusRabbitMQ/EventBusRabbitMQ.csproj", "EventBusRabbitMQ/"]
RUN dotnet restore "ESourcing.Order/ESourcing.Order.csproj"
COPY . .
WORKDIR "/src/ESourcing.Order"
RUN dotnet build "./ESourcing.Order.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ESourcing.Order.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ESourcing.Order.dll"]
