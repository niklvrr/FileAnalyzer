using FileAnalyzer.Domain.Entities;

namespace FileAnalyzer.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса для работы с конфигурацией
/// </summary>
public interface IConfigService
{
    /// <summary>
    /// Получить текущую конфигурацию
    /// </summary>
    Task<ConfigEntry?> GetConfigAsync();
    
    /// <summary>
    /// Сохранить конфигурацию
    /// </summary>
    Task SaveConfigAsync(ConfigEntry config);
    
    /// <summary>
    /// Получить конфигурацию по умолчанию
    /// </summary>
    Task<ConfigEntry> GetDefaultConfigAsync();
}

