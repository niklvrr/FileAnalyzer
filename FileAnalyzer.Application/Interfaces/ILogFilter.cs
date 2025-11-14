using FileAnalyzer.Domain.Entities;

namespace FileAnalyzer.Application.Interfaces;

/// <summary>
/// Интерфейс для фильтрации логов
/// </summary>
public interface ILogFilter
{
    /// <summary>
    /// Фильтрует список логов согласно заданным критериям
    /// </summary>
    List<LogEntry> Filter(List<LogEntry> logEntries);
    
    /// <summary>
    /// Устанавливает параметры фильтрации
    /// </summary>
    void SetFilterField();
}

