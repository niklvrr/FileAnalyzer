using FileAnalyzer.Application.Interfaces;
using FileAnalyzer.Application.Services;
using FileAnalyzer.Domain.Entities;
using FileAnalyzer.Infrastructure.Data;

namespace FileAnalyzer.ConsoleApp;

/// <summary>
/// Основное консольное приложение с меню
/// </summary>
public class ConsoleApplication
{
    private readonly ILogRepository _logRepository;
    private readonly ILogParser _logParser;
    private readonly IConfigService _configService;
    private readonly LogFilterService _filterService;
    private readonly LogSorterService _sorterService;
    private readonly LogStatisticsService _statisticsService;
    private readonly LogAnalyzerDbContext _dbContext;

    public ConsoleApplication(
        ILogRepository logRepository,
        ILogParser logParser,
        IConfigService configService,
        LogFilterService filterService,
        LogSorterService sorterService,
        LogStatisticsService statisticsService,
        LogAnalyzerDbContext dbContext)
    {
        _logRepository = logRepository;
        _logParser = logParser;
        _configService = configService;
        _filterService = filterService;
        _sorterService = sorterService;
        _statisticsService = statisticsService;
        _dbContext = dbContext;
    }

    public async Task RunAsync()
    {
        // Инициализация БД
        await _dbContext.Database.EnsureCreatedAsync();
        
        Console.WriteLine("═══════════════════════════════════════════════");
        Console.WriteLine("   Лог-Анализатор (Clean Architecture)");
        Console.WriteLine("═══════════════════════════════════════════════\n");

        bool exit = false;
        while (!exit)
        {
            ShowMenu();
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await LoadLogsFromFiles();
                    break;
                case "2":
                    await ConfigureSettings();
                    break;
                case "3":
                    await FilterLogs();
                    break;
                case "4":
                    await SortLogs();
                    break;
                case "5":
                    await ShowStatistics();
                    break;
                case "6":
                    await DisplayLogs();
                    break;
                case "7":
                    await ClearDatabase();
                    break;
                case "8":
                    exit = true;
                    Console.WriteLine("\nВыход из приложения...");
                    break;
                default:
                    Console.WriteLine("\n❌ Неверный выбор. Попробуйте снова.\n");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("\n═══════════════ ГЛАВНОЕ МЕНЮ ═══════════════");
        Console.WriteLine("1. 📁 Загрузить логи из файлов");
        Console.WriteLine("2. ⚙️  Настроить формат ввода/вывода");
        Console.WriteLine("3. 🔍 Отфильтровать логи");
        Console.WriteLine("4. 📊 Отсортировать логи");
        Console.WriteLine("5. 📈 Показать статистику");
        Console.WriteLine("6. 📄 Вывести логи");
        Console.WriteLine("7. 🗑️  Очистить базу данных");
        Console.WriteLine("8. 🚪 Выход");
        Console.WriteLine("═══════════════════════════════════════════");
        Console.Write("\nВыберите действие: ");
    }

    private async Task LoadLogsFromFiles()
    {
        Console.WriteLine("\n📁 ЗАГРУЗКА ЛОГОВ ИЗ ФАЙЛОВ");
        Console.WriteLine("────────────────────────────────────────");
        Console.Write("Введите пути к файлам через запятую: ");
        var input = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("❌ Пути не указаны.");
            return;
        }

        var filePaths = input.Split(',').Select(p => p.Trim()).ToList();
        var logs = await _logParser.ParseFromFilesAsync(filePaths);

        if (logs == null || logs.Count == 0)
        {
            Console.WriteLine("❌ Не удалось прочитать логи из файлов.");
            return;
        }

        await _logRepository.AddRangeAsync(logs);
        Console.WriteLine($"✅ Успешно загружено {logs.Count} записей в базу данных!");
    }

    private async Task ConfigureSettings()
    {
        Console.WriteLine("\n⚙️  НАСТРОЙКА ФОРМАТА");
        Console.WriteLine("────────────────────────────────────────");
        
        var config = await _configService.GetConfigAsync();
        
        Console.WriteLine($"\nТекущая конфигурация:");
        Console.WriteLine($"  Разделитель: {config?.Separator}");
        Console.WriteLine($"  Формат даты: {config?.DateFormat}");
        Console.WriteLine($"  Порядок полей: {string.Join(", ", config?.FieldsOrder ?? Array.Empty<string>())}");
        
        Console.Write("\n\nИзменить конфигурацию? (y/n): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            Console.Write("Введите разделитель (например, ' | ' или '[]'): ");
            var separator = Console.ReadLine() ?? " | ";
            
            Console.Write("Введите формат даты (например, 'yyyy-MM-dd HH:mm:ss'): ");
            var dateFormat = Console.ReadLine() ?? "yyyy-MM-dd HH:mm:ss";

            var newConfig = new ConfigEntry
            {
                Separator = separator,
                DateFormat = dateFormat,
                FieldsOrder = new[] { "Дата", "Уровень", "Сообщение" }
            };

            await _configService.SaveConfigAsync(newConfig);
            Console.WriteLine("✅ Конфигурация сохранена!");
        }
    }

    private async Task FilterLogs()
    {
        Console.WriteLine("\n🔍 ФИЛЬТРАЦИЯ ЛОГОВ");
        Console.WriteLine("────────────────────────────────────────");
        
        Console.Write("Начальная дата (yyyy-MM-dd или Enter для пропуска): ");
        DateTime? startDate = null;
        var startInput = Console.ReadLine();
        if (DateTime.TryParse(startInput, out var start))
            startDate = start;

        Console.Write("Конечная дата (yyyy-MM-dd или Enter для пропуска): ");
        DateTime? endDate = null;
        var endInput = Console.ReadLine();
        if (DateTime.TryParse(endInput, out var end))
            endDate = end;

        Console.Write("Уровень (Info/Warning/Error или Enter для пропуска): ");
        var level = Console.ReadLine();

        Console.Write("Ключевое слово в сообщении (или Enter для пропуска): ");
        var keyword = Console.ReadLine();

        var logs = await _logRepository.GetFilteredAsync(startDate, endDate, level, keyword);
        
        Console.WriteLine($"\n✅ Найдено {logs.Count()} записей:");
        DisplayLogsList(logs.ToList());
    }

    private async Task SortLogs()
    {
        Console.WriteLine("\n📊 СОРТИРОВКА ЛОГОВ");
        Console.WriteLine("────────────────────────────────────────");
        Console.WriteLine("Поле для сортировки:");
        Console.WriteLine("  1. По дате (date)");
        Console.WriteLine("  2. По уровню (level)");
        Console.WriteLine("  3. По длине сообщения (message)");
        Console.Write("\nВыберите поле: ");
        
        var fieldChoice = Console.ReadLine();
        var field = fieldChoice switch
        {
            "1" => "date",
            "2" => "level",
            "3" => "message",
            _ => "date"
        };

        Console.Write("Порядок сортировки (1 - возрастание, 2 - убывание): ");
        var orderChoice = Console.ReadLine();
        var ascending = orderChoice == "1";

        var allLogs = (await _logRepository.GetAllAsync()).ToList();
        var sorted = ascending
            ? _sorterService.AscendingSort(allLogs, field)
            : _sorterService.DescendingSort(allLogs, field);

        Console.WriteLine($"\n✅ Отсортировано {sorted.Count} записей:");
        DisplayLogsList(sorted);
    }

    private async Task ShowStatistics()
    {
        Console.WriteLine("\n📈 СТАТИСТИКА ПО ЛОГАМ");
        Console.WriteLine("────────────────────────────────────────");
        
        var logs = (await _logRepository.GetAllAsync()).ToList();
        var stats = _statisticsService.GetStatistics(logs);

        Console.WriteLine($"\n  Общее количество записей: {stats.TotalCount}");
        Console.WriteLine($"  Средняя длина сообщения: {stats.AverageMessageLength:F2} символов");
        
        if (stats.FirstLogDate.HasValue && stats.LastLogDate.HasValue)
        {
            Console.WriteLine($"  Первый лог: {stats.FirstLogDate:yyyy-MM-dd HH:mm:ss}");
            Console.WriteLine($"  Последний лог: {stats.LastLogDate:yyyy-MM-dd HH:mm:ss}");
        }

        Console.WriteLine("\n  Распределение по уровням:");
        foreach (var kvp in stats.LevelDistribution)
        {
            Console.WriteLine($"    - {kvp.Key}: {kvp.Value} записей");
        }
    }

    private async Task DisplayLogs()
    {
        Console.WriteLine("\n📄 ВЫВОД ЛОГОВ");
        Console.WriteLine("────────────────────────────────────────");
        Console.Write("Количество записей для отображения (Enter для всех): ");
        var countInput = Console.ReadLine();
        
        var logs = (await _logRepository.GetAllAsync()).ToList();
        
        if (int.TryParse(countInput, out int count) && count > 0)
        {
            logs = logs.Take(count).ToList();
        }

        DisplayLogsList(logs);
        
        Console.Write("\n\nСохранить в файл? (y/n): ");
        if (Console.ReadLine()?.ToLower() == "y")
        {
            Console.Write("Путь к файлу: ");
            var path = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(path))
            {
                var config = await _configService.GetConfigAsync() ?? await _configService.GetDefaultConfigAsync();
                var content = _logParser.FormatLogsForFile(logs, config);
                await File.WriteAllTextAsync(path, content);
                Console.WriteLine("✅ Логи сохранены в файл!");
            }
        }
    }

    private async Task ClearDatabase()
    {
        Console.WriteLine("\n🗑️  ОЧИСТКА БАЗЫ ДАННЫХ");
        Console.WriteLine("────────────────────────────────────────");
        Console.Write("Вы уверены? Это удалит все логи! (yes для подтверждения): ");
        
        if (Console.ReadLine()?.ToLower() == "yes")
        {
            await _logRepository.ClearAllAsync();
            Console.WriteLine("✅ База данных очищена!");
        }
        else
        {
            Console.WriteLine("❌ Операция отменена.");
        }
    }

    private void DisplayLogsList(List<LogEntry> logs)
    {
        if (logs.Count == 0)
        {
            Console.WriteLine("\n  Записи отсутствуют.");
            return;
        }

        Console.WriteLine($"\n  Всего записей: {logs.Count}\n");
        foreach (var log in logs.Take(10))
        {
            Console.WriteLine($"  [{log.Date:yyyy-MM-dd HH:mm:ss}] [{log.Level}] {log.FileName}");
            Console.WriteLine($"    {log.Message}");
            Console.WriteLine();
        }

        if (logs.Count > 10)
        {
            Console.WriteLine($"  ... и еще {logs.Count - 10} записей");
        }
    }
}

