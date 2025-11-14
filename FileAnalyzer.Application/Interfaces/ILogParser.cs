using FileAnalyzer.Domain.Entities;

namespace FileAnalyzer.Application.Interfaces;

/// <summary>
/// Интерфейс для парсинга логов из файлов
/// </summary>
public interface ILogParser
{
    /// <summary>
    /// Читает и парсит логи из файлов
    /// </summary>
    Task<List<LogEntry>?> ParseFromFilesAsync(IEnumerable<string> filePaths);
    
    /// <summary>
    /// Форматирует логи для записи в файл
    /// </summary>
    string FormatLogsForFile(List<LogEntry> logs, ConfigEntry config);
    
    /// <summary>
    /// Форматирует логи для вывода в консоль
    /// </summary>
    string FormatLogsForConsole(List<LogEntry> logs, ConfigEntry config);
}

