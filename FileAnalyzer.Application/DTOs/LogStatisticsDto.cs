namespace FileAnalyzer.Application.DTOs;

/// <summary>
/// DTO для статистики логов
/// </summary>
public class LogStatisticsDto
{
    /// <summary>
    /// Общее количество записей
    /// </summary>
    public int TotalCount { get; set; }
    
    /// <summary>
    /// Распределение по уровням (уровень -> количество)
    /// </summary>
    public Dictionary<string, int> LevelDistribution { get; set; } = new();
    
    /// <summary>
    /// Средняя длина сообщения
    /// </summary>
    public double AverageMessageLength { get; set; }
    
    /// <summary>
    /// Дата первого лога
    /// </summary>
    public DateTime? FirstLogDate { get; set; }
    
    /// <summary>
    /// Дата последнего лога
    /// </summary>
    public DateTime? LastLogDate { get; set; }
}

