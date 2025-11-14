using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FileAnalyzer.Application.Interfaces;
using FileAnalyzer.Domain.Entities;

namespace FileAnalyzer.Infrastructure.Parsers;

/// <summary>
/// Парсер логов из файлов (адаптация существующей бизнес-логики)
/// </summary>
public class LogFileParser : ILogParser
{
    private readonly IConfigService _configService;

    public LogFileParser(IConfigService configService)
    {
        _configService = configService;
    }

    /// <summary>
    /// Читает и парсит логи из файлов
    /// </summary>
    public async Task<List<LogEntry>?> ParseFromFilesAsync(IEnumerable<string> filePaths)
    {
        var allLogs = new List<LogEntry>();
        var config = await _configService.GetConfigAsync();
        
        if (config == null || string.IsNullOrWhiteSpace(config.Separator) 
            || string.IsNullOrWhiteSpace(config.DateFormat) 
            || config.FieldsOrder.Length == 0)
        {
            config = await _configService.GetDefaultConfigAsync();
        }

        foreach (var filePath in filePaths)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"Файл не найден: {filePath}");
                    continue;
                }

                string content = await File.ReadAllTextAsync(filePath, Encoding.UTF8);
                string[] lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = GetWrappedValues(line, config);
                    LogEntry? log = ParseLogEntry(parts, config, filePath);

                    if (log != null)
                        allLogs.Add(log);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла {filePath}: {ex.Message}");
            }
        }

        return allLogs.Count > 0 ? allLogs : null;
    }

    /// <summary>
    /// Извлекает значения из строки лога
    /// </summary>
    private static string[] GetWrappedValues(string stringData, ConfigEntry config)
    {
        string[] result;
        var sep = config.Separator;
        
        if (sep.Trim() is "[]" or "{}" or "()")
        {
            sep = sep.Trim();
            string openBracket = sep[0].ToString();
            string closeBracket = sep[1].ToString();
            string openEscaped = Regex.Escape(openBracket);
            string closeEscaped = Regex.Escape(closeBracket);
            string pattern = $@"{openEscaped}(.*?){closeEscaped}";
            var matches = Regex.Matches(stringData, pattern);
            string[] parts = new string[matches.Count + 1];

            for (int i = 0; i < matches.Count; i++)
            {
                parts[i] = matches[i].Groups[1].Value.Trim();
            }

            int lastCloseIndex = stringData.LastIndexOf(closeBracket, StringComparison.Ordinal);
            if (lastCloseIndex != -1 && lastCloseIndex < stringData.Length - 1)
            {
                parts[matches.Count] = stringData.Substring(lastCloseIndex + 1).Trim();
            }
            else
            {
                parts[matches.Count] = string.Empty;
            }

            return parts;
        }
        else
        {
            result = stringData.Split(sep);
        }

        return result;
    }

    /// <summary>
    /// Парсит одну лог-запись
    /// </summary>
    private LogEntry? ParseLogEntry(string[] parts, ConfigEntry config, string fileName)
    {
        try
        {
            var order = config.FieldsOrder;
            var log = new LogEntry
            {
                Id = Guid.NewGuid(),
                Date = DateTime.ParseExact(
                    parts[Array.IndexOf(order, "Дата")], 
                    config.DateFormat,
                    CultureInfo.InvariantCulture),
                Level = parts[Array.IndexOf(order, "Уровень")],
                Message = parts[Array.IndexOf(order, "Сообщение")],
                FileName = fileName,
                CreatedAt = DateTime.UtcNow
            };
            return log;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка парсинга лога: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Форматирует логи для записи в файл
    /// </summary>
    public string FormatLogsForFile(List<LogEntry> logs, ConfigEntry config)
    {
        var output = new StringBuilder();
        int indexLevel = Array.IndexOf(config.FieldsOrder, "Уровень");
        int indexDate = Array.IndexOf(config.FieldsOrder, "Дата");
        int indexMessage = Array.IndexOf(config.FieldsOrder, "Сообщение");

        foreach (var log in logs)
        {
            string[] parts = new string[3];
            parts[indexLevel] = log.Level;
            parts[indexDate] = log.Date.ToString(config.DateFormat);
            parts[indexMessage] = log.Message;

            string line;
            var sep = config.Separator.Trim();
            
            if (sep is "[]" or "{}" or "()")
            {
                char openBracket = sep[0];
                char closeBracket = sep[1];
                for (int i = 0; i < parts.Length - 1; i++)
                {
                    parts[i] = $"{openBracket}{parts[i]}{closeBracket}";
                }
                line = string.Join(" ", parts);
            }
            else
            {
                line = string.Join(sep, parts);
            }

            output.AppendLine(line);
        }

        return output.ToString();
    }

    /// <summary>
    /// Форматирует логи для вывода в консоль
    /// </summary>
    public string FormatLogsForConsole(List<LogEntry> logs, ConfigEntry config)
    {
        var output = new StringBuilder();
        output.AppendLine(new string('─', 50));
        var sep = config.Separator.Trim();

        foreach (var log in logs)
        {
            string line;
            if (sep is "[]" or "{}" or "()")
            {
                char openBracket = sep[0];
                char closeBracket = sep[1];
                line = $"{openBracket}{log.FileName}{closeBracket} {openBracket}{log.Date}{closeBracket} {openBracket}{log.Level}{closeBracket} \n {log.Message}";
            }
            else
            {
                line = $"{log.FileName} {sep} {log.Date} {sep} {log.Level} {sep} \n {log.Message}";
            }

            output.AppendLine(line);
            output.AppendLine(new string('─', 50));
        }

        return output.ToString();
    }
}

