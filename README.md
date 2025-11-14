# Лог-Анализатор (Clean Architecture)

Проект для анализа и обработки логов, реализованный с использованием принципов Clean Architecture, с поддержкой работы через ASP.NET Web API и консольное приложение.

## Оглавление
1. [Описание проекта](#описание-проекта)
2. [Архитектура](#архитектура)
3. [Функциональные возможности](#функциональные-возможности)
4. [Технологии](#технологии)
5. [Dependency Injection](#dependency-injection)
6. [Установка и запуск](#установка-и-запуск)
7. [API Endpoints](#api-endpoints)
8. [Использование консольного приложения](#использование-консольного-приложения)
9. [Структура проекта](#структура-проекта)
10. [База данных](#база-данных)
11. [Конфигурация](#конфигурация)

## Описание проекта

Данный проект представляет собой полнофункциональное приложение для анализа логов, построенное по принципам Clean Architecture. Приложение предоставляет два интерфейса для работы:
- REST API (ASP.NET Core Web API) для интеграции с другими системами
- Консольное приложение для интерактивной работы в терминале

### Ключевые особенности:
- Clean Architecture с четким разделением на слои (Domain, Application, Infrastructure, Presentation)
- Работа с базой данных SQLite для хранения и эффективного поиска логов
- REST API с документацией Swagger/OpenAPI
- Dependency Injection контейнер для управления зависимостями
- Гибкая конфигурация форматов ввода/вывода через JSON
- Инструменты фильтрации и сортировки для анализа логов
- Детальная статистика и аналитика по логам

## Архитектура

Проект построен по принципам Clean Architecture:

```
┌─────────────────────────────────────────────────┐
│         Presentation Layer                      │
│  ┌──────────────────┐  ┌──────────────────┐    │
│  │  FileAnalyzer.API│  │ ConsoleApp       │    │
│  │  (REST API)      │  │ (CLI Interface)  │    │
│  └──────────────────┘  └──────────────────┘    │
├─────────────────────────────────────────────────┤
│         Application Layer                       │
│  ┌──────────────────────────────────────────┐  │
│  │  Services, Interfaces, DTOs, Business    │  │
│  │  Logic (Filters, Sorters, Statistics)    │  │
│  └──────────────────────────────────────────┘  │
├─────────────────────────────────────────────────┤
│         Infrastructure Layer                    │
│  ┌──────────────────────────────────────────┐  │
│  │  SQLite Repository, File Parsers,        │  │
│  │  Config Service, EF Core DbContext       │  │
│  └──────────────────────────────────────────┘  │
├─────────────────────────────────────────────────┤
│         Domain Layer                            │
│  ┌──────────────────────────────────────────┐  │
│  │  Entities: LogEntry, ConfigEntry         │  │
│  │  Enums: LogLevel                         │  │
│  └──────────────────────────────────────────┘  │
└─────────────────────────────────────────────────┘
```

### Слои архитектуры:

#### 1. Domain Layer (FileAnalyzer.Domain)
Содержит бизнес-сущности (Entities) и не зависит от других слоев.
- Entities: `LogEntry`, `ConfigEntry`
- Enums: `LogLevel`

#### 2. Application Layer (FileAnalyzer.Application)
Содержит бизнес-логику приложения и определяет интерфейсы для работы с данными.
- Interfaces: `ILogRepository`, `ILogParser`, `IConfigService`, `ILogFilter`
- Services: `LogFilterService`, `LogSorterService`, `LogStatisticsService`
- DTOs: `LogStatisticsDto`

#### 3. Infrastructure Layer (FileAnalyzer.Infrastructure)
Реализация интерфейсов из Application Layer и работа с внешними зависимостями.
- Data: `LogAnalyzerDbContext` (Entity Framework Core)
- Repositories: `LogRepository` (SQLite)
- Parsers: `LogFileParser`
- Config: `ConfigService`

#### 4. Presentation Layer
- FileAnalyzer.API: ASP.NET Core Web API с Swagger
- FileAnalyzer_ConsoleApp: консольное приложение

## Функциональные возможности

### Чтение и парсинг логов
- Поддержка множества файлов формата .txt
- Автоматический парсинг согласно конфигурации
- Гибкая настройка разделителей и формата даты
- Извлечение данных с использованием регулярных выражений

### База данных SQLite
- Хранение логов в реляционной базе данных
- Быстрый поиск и фильтрация с использованием индексов
- Entity Framework Core для работы с данными
- Автоматическое создание базы данных при первом запуске

### Конфигурация
- Файлы конфигурации: `customConfig.json`, `defaultConfig.json`
- Настройка формата ввода/вывода
- Определение разделителей: ` | `, `[]`, `{}`, `()`
- Порядок полей: Дата, Уровень, Сообщение
- Формат даты (например, `yyyy-MM-dd HH:mm:ss`)

### Фильтрация
- По дате: диапазон дат (от - до)
- По уровню: INFO, WARNING, ERROR, DEBUG, FATAL
- По ключевым словам: поиск в сообщениях

### Сортировка
- По дате: возрастание/убывание
- По уровню важности: алфавитный порядок
- По длине сообщения: короткие/длинные

### Статистика
- Общее количество записей
- Распределение по уровням логирования
- Средняя длина сообщения
- Временной диапазон (первая/последняя запись)

### REST API
- Полный набор операций для работы с логами
- Swagger UI для тестирования API
- JSON формат данных
- CORS поддержка

### Консольный интерфейс
- Интерактивное меню
- Пошаговые инструкции
- Визуальное оформление результатов
- Сохранение результатов в файл

## Технологии

- .NET 8.0 - основной фреймворк
- ASP.NET Core Web API - REST API
- Entity Framework Core 8.0 - ORM для работы с БД
- SQLite - встраиваемая база данных
- Swagger/OpenAPI - документация API
- Microsoft.Extensions.DependencyInjection - DI контейнер

## Dependency Injection

Проект использует встроенный DI контейнер .NET для управления зависимостями и жизненным циклом объектов.

### Структура DI контейнера

Конфигурация DI вынесена в отдельные классы расширений:
- `FileAnalyzer.API/DependencyInjection/ServiceCollectionExtensions.cs`
- `FileAnalyzer_ConsoleApp/DependencyInjection/ServiceCollectionExtensions.cs`

### Регистрируемые сервисы

#### Контекст базы данных:
```csharp
services.AddDbContext<LogAnalyzerDbContext>(options =>
    options.UseSqlite(connectionString));
```

#### Репозитории:
```csharp
services.AddScoped<ILogRepository, LogRepository>();
```

#### Сервисы инфраструктуры:
```csharp
services.AddScoped<IConfigService, ConfigService>();
services.AddScoped<ILogParser, LogFileParser>();
```

#### Сервисы бизнес-логики:
```csharp
services.AddScoped<LogFilterService>();
services.AddScoped<LogSorterService>();
services.AddScoped<LogStatisticsService>();
```

### Время жизни сервисов

- **Scoped**: используется для DbContext, репозиториев и большинства сервисов
  - Создается один экземпляр на каждый HTTP-запрос (API) или операцию (Console)
  - Автоматическое управление транзакциями БД
  
- **Transient**: используется для ConsoleApplication
  - Создается новый экземпляр при каждом запросе
  - Легковесные объекты без состояния

### Использование DI в API

В `Program.cs`:
```csharp
builder.Services.AddApplicationServices(builder.Configuration);
```

В контроллерах внедрение через конструктор:
```csharp
public class LogsController : ControllerBase
{
    private readonly ILogRepository _logRepository;
    private readonly ILogParser _logParser;
    
    public LogsController(ILogRepository logRepository, ILogParser logParser)
    {
        _logRepository = logRepository;
        _logParser = logParser;
    }
}
```

### Использование DI в Console App

В `Program.cs`:
```csharp
var services = new ServiceCollection();
services.AddApplicationServices();
var serviceProvider = services.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<ConsoleApplication>();
await app.RunAsync();
```

### Преимущества использования DI

1. **Слабая связанность**: классы зависят от интерфейсов, а не от конкретных реализаций
2. **Тестируемость**: легко создавать mock-объекты для тестирования
3. **Гибкость**: простая замена реализаций без изменения кода
4. **Управление жизненным циклом**: автоматическое создание и уничтожение объектов
5. **Инверсия управления**: фреймворк управляет созданием зависимостей

## Установка и запуск

### Требования
- .NET 8.0 SDK
- Git

### Шаги установки

#### 1. Клонирование репозитория
```bash
git clone https://github.com/pomcheeek/FileAnalyzer.git
cd FileAnalyzer
```

#### 2. Восстановление зависимостей
```bash
dotnet restore
```

#### 3. Сборка проекта
```bash
dotnet build
```

#### 4. Запуск Web API
```bash
cd FileAnalyzer.API
dotnet run
```
API будет доступен по адресу: `https://localhost:5001` или `http://localhost:5000`

Swagger UI: `https://localhost:5001` (корневой URL)

#### 5. Запуск консольного приложения
```bash
cd FileAnalyzer_ConsoleApp
dotnet run
```

## API Endpoints

### Логи (Logs)

#### Получить все логи
```http
GET /api/logs
```

#### Получить лог по ID
```http
GET /api/logs/{id}
```

#### Загрузить логи из файлов
```http
POST /api/logs/upload
Content-Type: application/json

[
  "/path/to/file1.txt",
  "/path/to/file2.txt"
]
```

#### Получить отфильтрованные логи
```http
GET /api/logs/filter?startDate=2024-01-01&endDate=2024-12-31&level=Error&messageKeyword=exception
```

Параметры запроса:
- `startDate` (опционально): начальная дата в формате ISO 8601
- `endDate` (опционально): конечная дата в формате ISO 8601
- `level` (опционально): уровень логирования (Info, Warning, Error, Debug, Fatal)
- `messageKeyword` (опционально): ключевое слово для поиска в сообщениях

#### Получить отсортированные логи
```http
GET /api/logs/sorted?field=date&ascending=false
```

Параметры запроса:
- `field`: поле для сортировки (date, level, message)
- `ascending`: направление сортировки (true - возрастание, false - убывание)

#### Получить статистику
```http
GET /api/logs/statistics
```

Возвращает:
- Общее количество записей
- Распределение по уровням логирования
- Средняя длина сообщения
- Дата первого и последнего лога

#### Удалить лог
```http
DELETE /api/logs/{id}
```

#### Очистить все логи
```http
DELETE /api/logs
```

### Конфигурация (Config)

#### Получить текущую конфигурацию
```http
GET /api/config
```

#### Получить конфигурацию по умолчанию
```http
GET /api/config/default
```

#### Сохранить конфигурацию
```http
POST /api/config
Content-Type: application/json

{
  "fieldsOrder": ["Дата", "Уровень", "Сообщение"],
  "separator": " [] ",
  "dateFormat": "yyyy-MM-dd HH:mm:ss"
}
```

## Использование консольного приложения

После запуска консольного приложения отображается главное меню:

```
═══════════════ ГЛАВНОЕ МЕНЮ ═══════════════
1. Загрузить логи из файлов
2. Настроить формат ввода/вывода
3. Отфильтровать логи
4. Отсортировать логи
5. Показать статистику
6. Вывести логи
7. Очистить базу данных
8. Выход
═══════════════════════════════════════════
```

### Примеры использования

#### Загрузка логов
```
Выберите действие: 1
Введите пути к файлам через запятую: ./Files/Test1.txt,./Files/Test2.txt
Успешно загружено 150 записей в базу данных
```

#### Фильтрация
```
Выберите действие: 3
Начальная дата: 2024-01-01
Конечная дата: 2024-12-31
Уровень: Error
Ключевое слово: exception
Найдено 25 записей
```

#### Статистика
```
Выберите действие: 5

СТАТИСТИКА ПО ЛОГАМ
────────────────────────────────────────
  Общее количество записей: 150
  Средняя длина сообщения: 85.23 символов
  Первый лог: 2024-01-15 08:30:00
  Последний лог: 2024-11-14 18:45:00
  
  Распределение по уровням:
    - Info: 80 записей
    - Warning: 45 записей
    - Error: 25 записей
```

## Структура проекта

```
FileAnalyzer/
├── FileAnalyzer.Domain/              # Domain Layer
│   ├── Entities/
│   │   ├── LogEntry.cs              # Сущность лог-записи
│   │   └── ConfigEntry.cs           # Сущность конфигурации
│   └── Enums/
│       └── LogLevel.cs              # Перечисление уровней
│
├── FileAnalyzer.Application/         # Application Layer
│   ├── Interfaces/
│   │   ├── ILogRepository.cs        # Интерфейс репозитория
│   │   ├── ILogParser.cs            # Интерфейс парсера
│   │   ├── ILogFilter.cs            # Интерфейс фильтра
│   │   └── IConfigService.cs        # Интерфейс конфига
│   ├── Services/
│   │   ├── LogFilterService.cs      # Сервис фильтрации
│   │   ├── LogSorterService.cs      # Сервис сортировки
│   │   └── LogStatisticsService.cs  # Сервис статистики
│   └── DTOs/
│       └── LogStatisticsDto.cs      # DTO статистики
│
├── FileAnalyzer.Infrastructure/      # Infrastructure Layer
│   ├── Data/
│   │   └── LogAnalyzerDbContext.cs  # EF Core DbContext
│   ├── Repositories/
│   │   └── LogRepository.cs         # Реализация репозитория
│   ├── Parsers/
│   │   └── LogFileParser.cs         # Парсер файлов логов
│   └── Configs/
│       └── ConfigService.cs         # Сервис конфигурации
│
├── FileAnalyzer.API/                 # Presentation Layer - Web API
│   ├── Controllers/
│   │   ├── LogsController.cs        # API контроллер логов
│   │   └── ConfigController.cs      # API контроллер конфигов
│   ├── DependencyInjection/
│   │   └── ServiceCollectionExtensions.cs  # Настройка DI
│   ├── Program.cs                   # Точка входа API
│   └── appsettings.json            # Настройки приложения
│
├── FileAnalyzer_ConsoleApp/         # Presentation Layer - Console
│   ├── DependencyInjection/
│   │   └── ServiceCollectionExtensions.cs  # Настройка DI
│   ├── Program.cs                   # Точка входа консоли
│   ├── ConsoleApplication.cs        # Главное меню
│   └── Files/                       # Тестовые файлы логов
│       └── Test1.txt
│
└── README.md                        # Документация
```

## База данных

Проект использует SQLite для хранения логов. База данных создается автоматически при первом запуске.

### Структура таблицы Logs:
- `Id` (GUID) - уникальный идентификатор
- `Date` (DateTime) - дата и время лога
- `Level` (string) - уровень важности
- `Message` (string) - текст сообщения
- `FileName` (string) - имя исходного файла
- `CreatedAt` (DateTime) - дата добавления в БД

### Индексы:
- По полю `Date` - для быстрой сортировки по дате
- По полю `Level` - для фильтрации по уровню
- По полю `FileName` - для группировки по файлам

### Файлы баз данных:
- API: `loganalyzer.db`
- Console: `loganalyzer_console.db`

## Конфигурация

### Настройка строки подключения к БД

В `appsettings.json` (API):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=loganalyzer.db"
  }
}
```

В коде (Console):
```csharp
options.UseSqlite("Data Source=loganalyzer_console.db")
```

### Настройка формата логов

В `customConfig.json`:
```json
{
  "fieldsOrder": ["Дата", "Уровень", "Сообщение"],
  "separator": " | ",
  "dateFormat": "yyyy-MM-dd HH:mm:ss"
}
```

#### Параметры конфигурации:

- **fieldsOrder**: порядок полей в лог-файле (массив строк)
- **separator**: разделитель полей (строка)
  - Простые разделители: ` | `, ` - `, `,`
  - Скобки: `[]`, `{}`, `()`
- **dateFormat**: формат даты и времени (строка)
  - Примеры: `yyyy-MM-dd HH:mm:ss`, `dd.MM.yyyy HH:mm`, `MM/dd/yyyy HH:mm:ss`

### Формат файлов логов

#### Пример 1: Разделитель `[]`
```
[2024-11-14 10:30:00] [Error] Application crashed
[2024-11-14 10:31:15] [Info] Service restarted
```

#### Пример 2: Разделитель ` | `
```
2024-11-14 10:30:00 | Error | Application crashed
2024-11-14 10:31:15 | Info | Service restarted
```

## Принципы Clean Architecture

Проект следует принципам Clean Architecture:

1. **Независимость от фреймворков**: бизнес-логика не зависит от ASP.NET или EF Core
2. **Тестируемость**: бизнес-логика легко тестируется изолированно
3. **Независимость от UI**: можно использовать API или консоль
4. **Независимость от БД**: легко заменить SQLite на другую БД
5. **Независимость от внешних агентств**: бизнес-правила изолированы

### Зависимости между слоями:
```
Domain ← Application ← Infrastructure
                    ← Presentation (API, Console)
```

- Domain не зависит ни от чего
- Application зависит только от Domain
- Infrastructure зависит от Application и Domain
- Presentation зависит от всех слоев

### SOLID принципы

1. **Single Responsibility Principle**: каждый класс имеет одну ответственность
2. **Open/Closed Principle**: открыт для расширения, закрыт для изменения
3. **Liskov Substitution Principle**: интерфейсы легко заменяемы
4. **Interface Segregation Principle**: специализированные интерфейсы
5. **Dependency Inversion Principle**: зависимость от абстракций, не реализаций

## Автор

Николаев Роман (Nikolaev Roman)

---

**Примечание**: Данный проект демонстрирует применение Clean Architecture на практике и может служить шаблоном для создания других приложений с четким разделением ответственности между слоями.
