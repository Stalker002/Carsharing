# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Carsharing/Carsharing.csproj", "Carsharing/"]
COPY ["Carsharing.Application/Carsharing.Application.csproj", "Carsharing.Application/"]
COPY ["Carsharing.Core/Carsharing.Core.csproj", "Carsharing.Core/"]
COPY ["Carsharing.DataAccess/Carsharing.DataAccess.csproj", "Carsharing.DataAccess/"]
RUN dotnet restore "./Carsharing/Carsharing.csproj"
COPY . .
WORKDIR "/src/Carsharing"
RUN dotnet build "./Carsharing.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Carsharing.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
USER root
# wwwroot чтобы не было предупреждений
RUN mkdir -p /app/wwwroot/images/cars
RUN chown -R app:app /app/wwwroot
# Папка для DataProtection ключей
RUN mkdir -p /app/keys
RUN chown -R app:app /app/keys
COPY --from=publish /app/publish .
USER app
ENTRYPOINT ["dotnet", "Carsharing.dll"]