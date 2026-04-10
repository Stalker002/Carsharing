# Carsharing

Backend-сервис для системы каршеринга на ASP.NET Core.

Проект реализует REST API для клиентской части и административной панели. В API присутствуют операции для работы с автомобилями, категориями, клиентами, бронированиями, поездками, тарифами, платежами, штрафами, страховками, промокодами, отзывами, избранным и пользователями.

## Стек

- .NET 9 / ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- MinIO / S3 для хранения изображений
- JWT-аутентификация
- Swagger / OpenAPI
- Docker Compose

## Архитектура

Решение разделено на несколько проектов:

- `Carsharing` — веб-приложение, контроллеры, middleware, конфигурация API и аутентификации.
- `Carsharing.Application` — бизнес-логика, сервисы, DTO, маппинг.
- `Carsharing.Core` — доменные модели, перечисления, исключения, абстракции репозиториев и сервисных зависимостей.
- `Carsharing.DataAccess` — `DbContext`, entity-конфигурации, репозитории, работа с PostgreSQL.
- `Carsharing.Tests` — тестовый проект.

Структурно проект близок к Clean Architecture / layered architecture: API слой не работает с БД напрямую, а использует application-сервисы и репозитории через интерфейсы.

## Основные возможности

- управление автопарком и категориями автомобилей
- регистрация и управление клиентами
- бронирования и поездки
- тарифы и расчеты
- платежи, штрафы и страховки
- промокоды и отзывы
- избранные автомобили
- авторизация с ролями `admin` и `client`

## Аутентификация и авторизация

Используется JWT Bearer-аутентификация. Токен также читается из cookie `tasty`.

Политики доступа:

- `AdminPolicy` — только администратор
- `ClientPolicy` — только клиент
- `AdminClientPolicy` — администратор или клиент

## Хранение данных и файлов

- Основная база данных: PostgreSQL
- Изображения автомобилей: MinIO с S3-совместимым API
- Ключи Data Protection сохраняются в директорию `Carsharing/keys`

## Запуск через Docker Compose

В репозитории уже есть `docker-compose.yml`, который поднимает:

- `postgres`
- `minio`
- `carsharing`

Запуск:

```powershell
Copy-Item .env.example .env
docker compose up --build
```

Перед запуском заполните `.env` реальными секретами:

- `POSTGRES_PASSWORD`
- `JWT_SECRET_KEY`
- `MINIO_ROOT_USER`
- `MINIO_ROOT_PASSWORD`

`docker-compose.yml` по умолчанию поднимает приложение в окружении `Staging`, а не `Development`.

После запуска сервисы доступны по адресам:

- API: `http://localhost:5078`
- Swagger UI: `http://localhost:5078/swagger`
- MinIO API: `http://localhost:9000`
- MinIO Console: `http://localhost:9001`
- PostgreSQL: `localhost:5438`

## Локальный запуск без Docker

Требуется предварительно поднять PostgreSQL и MinIO, затем задать секреты через `UserSecrets` или переменные окружения.

Пример для `UserSecrets`:

```powershell
dotnet user-secrets set "ConnectionStrings:CarsharingDbContext" "Host=localhost;Port=5432;Database=Carsharing;Username=postgres;Password=<password>" --project .\Carsharing\Carsharing.csproj
dotnet user-secrets set "JwtOptions:SecretKey" "<long-random-secret>" --project .\Carsharing\Carsharing.csproj
dotnet user-secrets set "JwtOptions:ExpiresHours" "12" --project .\Carsharing\Carsharing.csproj
dotnet user-secrets set "Minio:AccessKey" "<minio-user>" --project .\Carsharing\Carsharing.csproj
dotnet user-secrets set "Minio:SecretKey" "<minio-password>" --project .\Carsharing\Carsharing.csproj
dotnet user-secrets set "Minio:ServiceURL" "http://localhost:9000" --project .\Carsharing\Carsharing.csproj
dotnet user-secrets set "Minio:PublicURL" "http://localhost:9000" --project .\Carsharing\Carsharing.csproj
dotnet user-secrets set "Minio:BucketName" "carsharing-images" --project .\Carsharing\Carsharing.csproj
dotnet user-secrets set "FileUpload:MaxCarImageBytes" "5242880" --project .\Carsharing\Carsharing.csproj
dotnet user-secrets set "FileUpload:MaxDocumentImageBytes" "10485760" --project .\Carsharing\Carsharing.csproj
```

Запуск приложения:

```powershell
dotnet restore
dotnet run --project .\Carsharing\Carsharing.csproj
```

## Конфигурация

Ключевые настройки находятся в:

- `Carsharing/appsettings.json`
- `Carsharing/appsettings.Development.json`
- `Carsharing/appsettings.Staging.json`
- `Carsharing/appsettings.Production.json`
- пользовательских секретах (`dotnet user-secrets`)
- переменных окружения / `.env`

Используются секции:

- `ConnectionStrings`
- `JwtOptions`
- `Minio`
- `FileUpload`

Во всех окружениях используется один и тот же ключ подключения к БД:

- `ConnectionStrings:CarsharingDbContext`
- env override: `ConnectionStrings__CarsharingDbContext`

Для файлов:

- изображения и документы хранятся в бакете `Minio:BucketName`
- загрузка валидирует допустимый формат файла и размер файла

## API

Основные контроллеры:

- `Cars`
- `Bookings`
- `Trips`
- `Clients`
- `Users`
- `Payments`
- `Fines`
- `Insurances`
- `Promocodes`
- `Reviews`
- `Bills`
- `Category`
- `Maintenance`
- `Favorite`
- `ClientDocuments`
- `Tariff`

Для просмотра и тестирования API можно использовать Swagger.

## Примечания

- При старте приложения вызывается `Database.Migrate()`.
- В development-режиме включены Swagger и CORS для фронтенда `http://localhost:5173`.
- В решении присутствует интеграционный тестовый проект `Carsharing.Tests`.
- Секреты не должны храниться в репозитории; после удаления из git их нужно отдельно ротировать во всех окружениях.
