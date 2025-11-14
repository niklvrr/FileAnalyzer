using FileAnalyzer.Application.Interfaces;
using FileAnalyzer.Application.Services;
using FileAnalyzer.Infrastructure.Configs;
using FileAnalyzer.Infrastructure.Data;
using FileAnalyzer.Infrastructure.Parsers;
using FileAnalyzer.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FileAnalyzer.API.DependencyInjection;

/// <summary>
/// Расширения для конфигурации Dependency Injection контейнера
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует все сервисы приложения в DI контейнере
    /// </summary>
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        // Регистрация контекста базы данных
        services.AddDbContext<LogAnalyzerDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection") 
                ?? "Data Source=loganalyzer.db"));

        // Регистрация репозиториев
        services.AddScoped<ILogRepository, LogRepository>();
        
        // Регистрация сервисов инфраструктуры
        services.AddScoped<IConfigService, ConfigService>();
        services.AddScoped<ILogParser, LogFileParser>();
        
        // Регистрация сервисов бизнес-логики
        services.AddScoped<LogFilterService>();
        services.AddScoped<LogSorterService>();
        services.AddScoped<LogStatisticsService>();

        return services;
    }
}

