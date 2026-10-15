# TouristRoutes

Учебный ASP.NET Core Web API проект для управления туристическими маршрутами.
Получен от наставника в виде ZIP-архива с рядом намеренных архитектурных
проблем; Цель - привести проект к production-ready виду, сохранив историю
коммитов с пошаговым рефакторингом.

## Стек

- **Runtime:** .NET 10
- **Framework:** ASP.NET Core Web API
- **Language:** C# 14
- **Логирование:** Serilog (Console + File)
- **JSON:** System.Text.Json с JsonStringEnumConverter

## Запуск

### Требования

- .NET SDK 10.0+
- Windows / Linux / macOS

### Установка зависимостей

```bash
dotnet restore
```

### Запуск

```bash
dotnet run
```

По умолчанию слушает `http://localhost:5020` (см. `Properties/launchSettings.json`).

## Архитектура

После рефакторинга проект следует принципу единой ответственности (SRP).
`RouteService` - тонкий Facade, делегирующий вызовы специализированным
сервисам:

```
RoutesController
    ↓
IRouteService (Facade)
    ├── IRouteRepository - доступ к данным (заглушка)
    ├── INotificationService - email/SMS/DB логирование
    ├── IGpxParser - парсинг GPX XML
    ├── IMapService - HTTP-запросы к внешнему API карт (IHttpClientFactory)
    └── IRoutePricingService - расчёт стоимости
```

### Lifetime сервисов

| Сервис | Lifetime | Причина |
|---|---|---|
| `IRouteService` | Scoped | Facade, зависит от Scoped сервисов |
| `IRouteRepository` | Scoped | Готов к замене на DbContext (Scoped) |
| `INotificationService` | Scoped | В будущем будет зависеть от DbContext |
| `IMapService` | Transient | Регистрируется через `AddHttpClient<>()` (typed client) |
| `IRoutePricingService` | Singleton | Stateless pure logic |
| `IGpxParser` | Singleton | Stateless pure logic |

## API Endpoints

### GET `/api/Routes/{id}`

Возвращает маршрут по идентификатору.

**Параметры:**
- `id` (int, path) - идентификатор маршрута

**Ответ:** `200 OK` с `TouristRoute` JSON.

```json
{
  "id": 1,
  "name": "Test",
  "description": null,
  "gpxTrackData": null,
  "routeType": null,
  "waypoints": []
}
```

### POST `/api/Routes/upload-gpx?id={id}`

Сохраняет GPX-контент маршрута в файл и уведомляет подписчиков.

**Параметры:**
- `id` (int, query) - идентификатор маршрута, должен быть > 0
- `gpxData` (string, body) - содержимое GPX как JSON-строка в кавычках

**Пример запроса (PowerShell):**
```powershell
$body = '"<gpx></gpx>"'
Invoke-WebRequest -UseBasicParsing -Method Post `
  -Uri "http://localhost:5020/api/Routes/upload-gpx?id=1" `
  -Body $body -ContentType "application/json"
```

**Ответ:** `200 OK` (пустое тело).

### GET `/api/Routes/filter`

Фильтрует маршруты по критериям.

**Query-параметры** (все опциональные):

| Параметр | Тип | Валидация |
|---|---|---|
| `MinLat`, `MaxLat` | double | [-90, 90] |
| `MinLng`, `MaxLng` | double | [-180, 180] |
| `MinDurationDays`, `MaxDurationDays` | int | [0, 365] |
| `MinDifficulty`, `MaxDifficulty` | double | [0, 10] |
| `RegionName` | string | max 100 chars |
| `Season` | string | max 50 chars |

**Пример:**
```
GET /api/Routes/filter?MinLat=50&MaxLat=60&RegionName=Caucasus&Season=Summer
```

**Ответ:** `200 OK` с массивом `TouristRoute` (пока всегда `[]` (заглушка)).
При невалидных параметрах `400 Bad Request` с JSON-описанием ошибок.

## Логирование

Используется Serilog (Console + File):
- Консоль - для разработки.
- Файлы `logs/touristroutes-YYYYMMDD.log` - daily rolling.

Глобальный обработчик исключений `GlobalExceptionHandler` ловит все
необработанные исключения, логирует их и возвращает клиенту `500`
с JSON `{"error": "Internal server error", "traceId": "..."}`.

## Структура проекта

```
TouristRoutes/
├── Controllers/
│   └── RoutesController.cs       # API endpoints
├── Services/
│   ├── IRouteService.cs          # Facade interface
│   ├── RouteService.cs           # Facade implementation
│   ├── IRouteRepository.cs       # Data access interface
│   ├── RouteRepository.cs        # Mock implementation
│   ├── IGpxParser.cs
│   ├── GpxParser.cs              # LINQ to XML based parser
│   ├── IMapService.cs
│   ├── MapService.cs             # IHttpClientFactory based
│   ├── IRoutePricingService.cs
│   ├── RoutePricingService.cs
│   ├── INotificationService.cs
│   └── NotificationService.cs
├── Middleware/
│   └── GlobalExceptionHandler.cs
├── Models.cs                     # TouristRoute, Coordinates, RouteFilterCriteria, RouteType
├── Program.cs                    # DI registration + middleware pipeline
├── appsettings.json              # non-secret config (placeholders only)
├── appsettings.Development.json  # secrets
├── TouristRoutes.csproj
└── TouristRoutes.slnx
```

## История рефакторинга

Проект получен с рядом архитектурных проблем.
Рефакторинг разбит на атомарные коммиты (19 коммитов, см. `git log`):

## Лицензия

Учебный проект.