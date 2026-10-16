# Инструкция по созданию Booking Service

## Цель

`Booking Service` выделяется из текущего монолита Carsharing как отдельный сервис, который отвечает за бронирование автомобиля до начала поездки.

Главная задача сервиса: надежно создать, отменить или завершить бронь так, чтобы один автомобиль не мог быть забронирован двумя клиентами одновременно, а клиент не мог иметь несколько активных броней.

В полном варианте микросервисной архитектуры `Booking Service` является отдельным сервисом. В облегченной миграции его можно временно объединить с `Trip Service` в `Booking/Trip Service`, но границы бронирования все равно нужно проектировать отдельно.

## Граница ответственности

`Booking Service` отвечает за:

- создание брони;
- проверку активной брони клиента;
- проверку срока действия брони;
- отмену брони;
- истечение брони по TTL;
- хранение истории бронирований;
- публикацию событий о состоянии брони.

`Booking Service` не должен отвечать за:

- регистрацию и авторизацию пользователей;
- хранение полного профиля клиента;
- управление автомобилями, категориями и фотографиями;
- жизненный цикл поездки после старта;
- расчет стоимости поездки;
- выставление счетов и оплату;
- штрафы и нарушения.

## Что переносится из монолита

Из текущего проекта переносятся:

- `BookingsController`;
- `BookingsService`;
- `IBookingsService`;
- `BookingRepository`;
- `IBookingRepository`;
- `Booking`;
- `BookingStatus`;
- DTO из `Shared.Contracts.Bookings`;
- enum `BookingStatusEnum`, если он используется только бронированием.

Текущий код уже содержит важную бизнес-логику, которую нужно сохранить:

- создание брони только для существующего клиента;
- запрет активной брони, если у клиента уже есть активная бронь;
- перевод автомобиля из `Available` в `Reserved` при создании брони;
- обработка конфликта, если автомобиль уже недоступен;
- ограничение доступа клиента только к своим броням.

## Рекомендуемая структура проекта

На первом этапе достаточно одного API-проекта:

```text
src/
  Services/
    Booking/
      Carsharing.Booking.Api/
        Controllers/
        Contracts/
        Data/
        Domain/
        Services/
        Integration/
        Program.cs
```

Если нужно сразу сделать более чистое разделение по слоям:

```text
src/
  Services/
    Booking/
      Carsharing.Booking.Api/
      Carsharing.Booking.Application/
      Carsharing.Booking.Domain/
      Carsharing.Booking.Infrastructure/
```

Для текущего проекта практичнее начать с одного `Carsharing.Booking.Api`, а затем разделить его на слои, когда сервис вырастет.

## Модель данных

Минимальная сущность `Booking`:

| Поле | Тип | Назначение |
| --- | --- | --- |
| `Id` | `int` или `Guid` | Идентификатор брони. |
| `Status` | `BookingStatus` | Статус брони. |
| `CarId` | `int` или `Guid` | Идентификатор автомобиля из Fleet Service. |
| `ClientId` | `int` или `Guid` | Идентификатор клиента из Customer/Profile Service или монолита на переходном этапе. |
| `StartTime` | `DateTimeOffset` | Время создания или начала брони. |
| `EndTime` | `DateTimeOffset?` | Время завершения, отмены или истечения брони. |
| `ExpiresAt` | `DateTimeOffset` | Момент автоматического истечения брони. |
| `CancellationReason` | `string?` | Причина отмены. |
| `CreatedAt` | `DateTimeOffset` | Дата создания записи. |
| `UpdatedAt` | `DateTimeOffset?` | Дата последнего изменения. |
| `RowVersion` | `byte[]` или `uint` | Поле конкурентного обновления. |

Рекомендуемые статусы:

```text
Active
Confirmed
Cancelled
Expired
Completed
```

Если используется текущий `BookingStatusEnum`, нужно привести значения к понятным именам и не завязывать бизнес-логику на магические числа.

## База данных

На переходном этапе можно использовать общую PostgreSQL-инстанцию, но отдельную схему:

```text
booking.bookings
booking.booking_statuses
booking.reservation_locks
booking.outbox_messages
booking.inbox_messages
```

Правило: `Booking Service` работает только со своей схемой. Он не читает напрямую таблицы автомобилей, клиентов, поездок, счетов или платежей.

Позже схему `booking` можно вынести в отдельную базу:

```text
carsharing_booking
```

## API сервиса

Минимальные endpoints:

```text
GET    /api/v1/bookings
GET    /api/v1/bookings/{id}
GET    /api/v1/bookings/my
GET    /api/v1/bookings/by-car/{carId}
POST   /api/v1/bookings
POST   /api/v1/bookings/{id}/cancel
POST   /api/v1/bookings/{id}/confirm
```

Административные операции лучше отделить:

```text
GET    /api/v1/admin/bookings
GET    /api/v1/admin/bookings/{id}
POST   /api/v1/admin/bookings/{id}/cancel
```

Не рекомендуется оставлять общий `PUT /bookings/{id}` для изменения любых полей. Для микросервисов лучше иметь явные команды: создать, отменить, подтвердить, истечь.

## Контракты

Минимальный запрос на создание брони:

```csharp
public sealed record CreateBookingRequest(
    int CarId,
    DateTimeOffset? StartTime,
    string? IdempotencyKey);
```

Минимальный ответ:

```csharp
public sealed record BookingResponse(
    int Id,
    int CarId,
    int ClientId,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpiresAt,
    DateTimeOffset? EndTime);
```

Если сервис уже отделен от Customer/Profile Service, `ClientId` не должен приходить из тела запроса. Он берется из JWT claims или через запрос к Profile Service.

## Взаимодействие с другими сервисами

### Identity Service

`Booking Service` принимает JWT и достает из него:

- `UserId`;
- роль;
- permissions.

Сам сервис не хранит пароли, refresh tokens и роли.

### Customer/Profile Service

Перед созданием брони нужно убедиться, что клиент существует и допущен к аренде.

Варианты:

- переходный этап: использовать текущую таблицу клиентов в монолите;
- зрелый вариант: REST-запрос `GET /api/v1/customers/by-user/{userId}`;
- более быстрый вариант для чтения: локальная read model, обновляемая событиями `CustomerVerified`, `CustomerBlocked`.

### Fleet Service

При создании брони нужно зарезервировать автомобиль.

Варианты:

- синхронный REST-запрос в Fleet Service: `POST /api/v1/cars/{carId}/reserve`;
- событие `BookingCreated`, на которое Fleet Service отвечает `CarReserved` или `CarReservationFailed`;
- переходный вариант внутри монолита: текущий `TryUpdateStatus(Available, Reserved)`.

Для первого выделения сервиса проще использовать синхронный запрос или временный адаптер к монолиту. Для более зрелой архитектуры нужен event-based flow с идемпотентностью.

### Trip Service

`Trip Service` стартует поездку только из активной брони.

Варианты проверки:

- синхронный запрос `GET /api/v1/bookings/{id}`;
- локальная read model в Trip Service;
- событие `BookingConfirmed`, если старт поездки подтверждает бронь.

После старта поездки `Booking Service` переводит бронь в `Confirmed` или `Completed`, чтобы она больше не считалась активной бронью.

## События

Минимальный набор событий:

```text
BookingCreated
BookingCancelled
BookingExpired
BookingConfirmed
```

Рекомендуемый envelope:

```csharp
public sealed record IntegrationEvent<TPayload>(
    Guid EventId,
    string EventType,
    DateTimeOffset OccurredAt,
    string CorrelationId,
    string? CausationId,
    int Version,
    TPayload Payload);
```

Пример payload:

```csharp
public sealed record BookingCreatedPayload(
    int BookingId,
    int CarId,
    int ClientId,
    DateTimeOffset CreatedAt,
    DateTimeOffset ExpiresAt);
```

События должны публиковаться через outbox, а не напрямую из бизнес-метода после `SaveChanges`.

## Бизнес-правила

Обязательные правила:

- клиент может иметь только одну активную бронь;
- автомобиль может иметь только одну активную бронь;
- бронь имеет TTL, например 15 минут;
- бронь истекает автоматически, если поездка не начата;
- отмененная или истекшая бронь освобождает автомобиль;
- создание брони должно быть идемпотентным;
- повторный запрос с тем же `IdempotencyKey` не должен создавать вторую бронь;
- клиент может смотреть только свои брони;
- администратор может смотреть все брони.

Правило по времени нужно исправить при переносе: текущая модель запрещает `StartTime` в будущем, но для бронирования это спорное ограничение. Для каршеринга лучше хранить `CreatedAt`, `ExpiresAt`, а `StartTime` использовать только если есть сценарий отложенной брони.

## Защита от двойного бронирования

Основной риск `Booking Service` - гонка двух запросов на один автомобиль.

Минимальная защита:

- транзакция при создании брони;
- уникальный индекс на активную бронь автомобиля;
- уникальный индекс на активную бронь клиента;
- optimistic concurrency через `RowVersion`;
- идемпотентность по ключу запроса.

Пример логических ограничений:

```text
unique active booking by car:
  CarId where Status = Active

unique active booking by client:
  ClientId where Status = Active

unique idempotency:
  ClientId + IdempotencyKey
```

Если используется PostgreSQL, можно сделать partial unique index:

```sql
CREATE UNIQUE INDEX ux_bookings_active_car
ON booking.bookings (car_id)
WHERE status = 'Active';

CREATE UNIQUE INDEX ux_bookings_active_client
ON booking.bookings (client_id)
WHERE status = 'Active';
```

## Фоновая обработка истечения броней

Нужен background worker:

```text
BookingExpirationWorker
```

Он должен:

- регулярно находить активные брони с `ExpiresAt <= now`;
- переводить их в `Expired`;
- публиковать `BookingExpired`;
- вызывать Fleet Service или отправлять событие для освобождения автомобиля;
- быть идемпотентным при повторном запуске.

На первом этапе worker можно держать внутри `Carsharing.Booking.Api` как `BackgroundService`.

## Переходный план выделения

### Шаг 1. Подготовить контракты

- Перенести DTO бронирования в общий проект контрактов.
- Разделить request/response модели API и внутреннюю доменную модель.
- Добавить контракты событий `BookingCreated`, `BookingCancelled`, `BookingExpired`, `BookingConfirmed`.

### Шаг 2. Уточнить доменную модель

- Добавить `ExpiresAt`.
- Добавить явные статусы.
- Добавить `CancellationReason`.
- Добавить `CreatedAt` и `UpdatedAt`.
- Добавить concurrency token.
- Перенести проверку активной брони из repository в application/domain service.

### Шаг 3. Создать проект сервиса

- Создать `Carsharing.Booking.Api`.
- Настроить ASP.NET Core, Swagger, health checks.
- Подключить EF Core.
- Настроить отдельную схему `booking`.
- Добавить миграции.

### Шаг 4. Перенести API

- Перенести `BookingsController`.
- Заменить общий `PUT` на явные команды.
- Оставить совместимые маршруты через API Gateway, если мобильное приложение уже использует старые endpoints.

### Шаг 5. Настроить интеграции

- JWT validation через Identity Service.
- Проверка клиента через Customer/Profile Service или временный адаптер к монолиту.
- Резервирование автомобиля через Fleet Service или временный адаптер к монолиту.
- Публикация событий через RabbitMQ.

### Шаг 6. Добавить outbox

- Сохранять события в `booking.outbox_messages` в той же транзакции, что и бронь.
- Отдельный publisher отправляет события в RabbitMQ.
- После успешной отправки помечать событие как опубликованное.

### Шаг 7. Переключить маршрутизацию

- Добавить маршрут в API Gateway.
- Перенаправить `/bookings` на `Booking Service`.
- Оставить старый контроллер в монолите выключенным или только как совместимый proxy на время миграции.

### Шаг 8. Покрыть проверками

Минимальные тесты:

- создание брони для доступного автомобиля;
- ошибка при бронировании недоступного автомобиля;
- ошибка при второй активной брони клиента;
- конкурентное бронирование одного автомобиля двумя клиентами;
- отмена брони освобождает автомобиль;
- истечение брони освобождает автомобиль;
- клиент не видит чужую бронь;
- повторный `IdempotencyKey` возвращает существующую бронь.

## Что оставить в монолите на переходном этапе

Временно можно оставить:

- `Trip Service`;
- `Billing Service`;
- `Payment Service`;
- `Fine Service`;
- административные экраны;
- старые read-only запросы для отчетов.

Но после выделения `Booking Service` монолит не должен напрямую менять таблицу бронирований. Все изменения брони должны идти через API сервиса или события.

## Минимальный результат

Сервис считается созданным, когда выполнены условия:

- есть отдельный проект `Carsharing.Booking.Api`;
- у сервиса есть своя схема или база;
- создание брони защищено от двойного бронирования;
- бронь имеет TTL;
- есть API создания, просмотра, отмены и подтверждения брони;
- есть события `BookingCreated`, `BookingCancelled`, `BookingExpired`;
- API Gateway маршрутизирует запросы бронирования в новый сервис;
- текущий сценарий мобильного приложения продолжает работать.

## Частые ошибки

- Оставить `Booking Service` напрямую читающим таблицы `Cars` и `Clients`.
- Делать распределенную транзакцию между Booking и Fleet.
- Публиковать события до успешного сохранения брони.
- Использовать общий `PUT`, который позволяет поменять `CarId`, `ClientId` и статус без бизнес-команды.
- Не добавить уникальные ограничения на активную бронь автомобиля и клиента.
- Считать бронь активной бесконечно без TTL.
- Хранить бизнес-сущности всех сервисов в общем `Shared.Domain`.
