using FileAnalyzer.Domain.Entities;

namespace FileAnalyzer.Application.Interfaces;

/// <summary>
/// Интерфейс репозитория для работы с логами
/// </summary>
public interface ILogRepository
{
    /// <summary>
    /// Получить все логи
    /// </summary>
    Task<IEnumerable<LogEntry>> GetAllAsync();
    
    /// <summary>
    /// Получить лог по идентификатору
    /// </summary>
    Task<LogEntry?> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Добавить логи
    /// </summary>
    Task AddRangeAsync(IEnumerable<LogEntry> logs);
    
    /// <summary>
    /// Удалить лог
    /// </summary>
    Task DeleteAsync(Guid id);
    
    /// <summary>
    /// Очистить все логи
    /// </summary>
    Task ClearAllAsync();
    
    /// <summary>
    /// Получить логи с фильтрацией
    /// </summary>
    Task<IEnumerable<LogEntry>> GetFilteredAsync(DateTime? startDate, DateTime? endDate, string? level, string? messageKeyword);
}

