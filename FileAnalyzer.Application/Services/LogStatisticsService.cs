using FileAnalyzer.Application.DTOs;
using FileAnalyzer.Domain.Entities;

namespace FileAnalyzer.Application.Services;

/// <summary>
/// Сервис для вычисления статистики по логам
/// </summary>
public class LogStatisticsService
{
    /// <summary>
    /// Получить статистику по логам
    /// </summary>
    public LogStatisticsDto GetStatistics(List<LogEntry> logs)
    {
        if (logs == null || logs.Count == 0)
        {
            return new LogStatisticsDto
            {
                TotalCount = 0,
                LevelDistribution = new Dictionary<string, int>(),
                AverageMessageLength = 0
            };
        }

        var levelDistribution = logs
            .GroupBy(log => log.Level)
            .ToDictionary(g => g.Key, g => g.Count());

        var averageMessageLength = logs.Average(log => log.Message.Length);

        return new LogStatisticsDto
        {
            TotalCount = logs.Count,
            LevelDistribution = levelDistribution,
            AverageMessageLength = averageMessageLength,
            FirstLogDate = logs.Min(log => log.Date),
            LastLogDate = logs.Max(log => log.Date)
        };
    }
}

