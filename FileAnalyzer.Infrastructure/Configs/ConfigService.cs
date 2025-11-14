using System.Text;
using System.Text.Json;
using FileAnalyzer.Application.Interfaces;
using FileAnalyzer.Domain.Entities;

namespace FileAnalyzer.Infrastructure.Configs;

/// <summary>
/// Сервис для работы с конфигурацией (адаптация существующей логики)
/// </summary>
public class ConfigService : IConfigService
{
    private readonly string _customConfigPath;
    private readonly string _defaultConfigPath;

    public ConfigService()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        _customConfigPath = Path.Combine(baseDir, "Configs", "customConfig.json");
        _defaultConfigPath = Path.Combine(baseDir, "Configs", "defaultConfig.json");
        
        // Создаем директорию для конфигов если её нет
        Directory.CreateDirectory(Path.Combine(baseDir, "Configs"));
    }

    public async Task<ConfigEntry?> GetConfigAsync()
    {
        try
        {
            if (!File.Exists(_customConfigPath))
            {
                return await GetDefaultConfigAsync();
            }

            string json = await File.ReadAllTextAsync(_customConfigPath, Encoding.UTF8);
            var config = JsonSerializer.Deserialize<ConfigEntry>(json);
            
            if (config == null || string.IsNullOrWhiteSpace(config.Separator) 
                || string.IsNullOrWhiteSpace(config.DateFormat) 
                || config.FieldsOrder.Length == 0)
            {
                return await GetDefaultConfigAsync();
            }

            return config;
        }
        catch
        {
            return await GetDefaultConfigAsync();
        }
    }

    public async Task SaveConfigAsync(ConfigEntry config)
    {
        var json = JsonSerializer.Serialize(config, new JsonSerializerOptions 
        { 
            WriteIndented = true 
        });
        await File.WriteAllTextAsync(_customConfigPath, json, Encoding.UTF8);
    }

    public async Task<ConfigEntry> GetDefaultConfigAsync()
    {
        var defaultConfig = new ConfigEntry
        {
            FieldsOrder = new[] { "Дата", "Уровень", "Сообщение" },
            Separator = " [] ",
            DateFormat = "yyyy-MM-dd HH:mm:ss"
        };

        // Создаем файл конфига по умолчанию если его нет
        if (!File.Exists(_defaultConfigPath))
        {
            var json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions 
            { 
                WriteIndented = true 
            });
            await File.WriteAllTextAsync(_defaultConfigPath, json, Encoding.UTF8);
        }

        return defaultConfig;
    }
}

