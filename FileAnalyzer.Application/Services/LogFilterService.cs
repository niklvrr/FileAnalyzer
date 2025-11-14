using FileAnalyzer.Application.Interfaces;
using FileAnalyzer.Domain.Entities;

namespace FileAnalyzer.Application.Services;

/// <summary>
/// Сервис для фильтрации логов (используется существующая бизнес-логика)
/// </summary>
public class LogFilterService
{
    /// <summary>
    /// Фильтрация по дате
    /// </summary>
    public List<LogEntry> FilterByDate(List<LogEntry> logs, DateTime startDate, DateTime endDate)
    {
        if (startDate == DateTime.MinValue && endDate == DateTime.MinValue)
            return logs;
            
        return logs.Where(log => log.Date >= startDate && log.Date <= endDate).ToList();
    }
    
    /// <summary>
    /// Фильтрация по уровню
    /// </summary>
    public List<LogEntry> FilterByLevel(List<LogEntry> logs, string level)
    {
        if (string.IsNullOrWhiteSpace(level))
            return logs;
            
        return logs.Where(log => log.Level.Equals(level, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    
    /// <summary>
    /// Фильтрация по ключевым словам в сообщении
    /// </summary>
    public List<LogEntry> FilterByMessage(List<LogEntry> logs, string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return logs;
            
        return logs.Where(log => log.Message.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    
    /// <summary>
    /// Применить все фильтры
    /// </summary>
    public List<LogEntry> ApplyFilters(List<LogEntry> logs, DateTime? startDate, DateTime? endDate, string? level, string? messageKeyword)
    {
        var filtered = logs;
        
        if (startDate.HasValue || endDate.HasValue)
        {
            filtered = FilterByDate(filtered, startDate ?? DateTime.MinValue, endDate ?? DateTime.MaxValue);
        }
        
        if (!string.IsNullOrWhiteSpace(level))
        {
            filtered = FilterByLevel(filtered, level);
        }
        
        if (!string.IsNullOrWhiteSpace(messageKeyword))
        {
            filtered = FilterByMessage(filtered, messageKeyword);
        }
        
        return filtered;
    }
}

