using FileAnalyzer.Domain.Entities;

namespace FileAnalyzer.Application.Services;

/// <summary>
/// Сервис для сортировки логов (используется существующая бизнес-логика)
/// </summary>
public class LogSorterService
{
    /// <summary>
    /// Сортирует список логов по заданному полю в порядке возрастания
    /// </summary>
    public List<LogEntry> AscendingSort(List<LogEntry> logs, string field)
    {
        return field.ToLower() switch
        {
            "date" => logs.OrderBy(log => log.Date).ToList(),
            "level" => logs.OrderBy(log => log.Level).ToList(),
            "message" => logs.OrderBy(log => log.Message.Length).ToList(),
            _ => logs
        };
    }

    /// <summary>
    /// Сортирует список логов по заданному полю в порядке убывания
    /// </summary>
    public List<LogEntry> DescendingSort(List<LogEntry> logs, string field)
    {
        return field.ToLower() switch
        {
            "date" => logs.OrderByDescending(log => log.Date).ToList(),
            "level" => logs.OrderByDescending(log => log.Level).ToList(),
            "message" => logs.OrderByDescending(log => log.Message.Length).ToList(),
            _ => logs
        };
    }
}

