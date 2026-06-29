FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5011
EXPOSE 5012

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["NotificationService/QNotificationService.API/QNotificationService.API.csproj", "QNotificationService.API/"]
COPY ["NotificationService/QNotificationService.Application/QNotificationService.Application.csproj", "QNotificationService.Application/"]
COPY ["NotificationService/QNotificationService.Contracts/QNotificationService.Contracts.csproj", "QNotificationService.Contracts/"]
RUN dotnet restore "QNotificationService.API/QNotificationService.API.csproj"
COPY . .
WORKDIR "/src/NotificationService/QNotificationService.API"
RUN dotnet build "./QNotificationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./QNotificationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QNotificationService.API.dll"]
