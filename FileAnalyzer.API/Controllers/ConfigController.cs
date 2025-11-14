using FileAnalyzer.Application.Interfaces;
using FileAnalyzer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalyzer.API.Controllers;

/// <summary>
/// Контроллер для работы с конфигурацией
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ConfigController : ControllerBase
{
    private readonly IConfigService _configService;

    public ConfigController(IConfigService configService)
    {
        _configService = configService;
    }

    /// <summary>
    /// Получить текущую конфигурацию
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ConfigEntry>> GetConfig()
    {
        var config = await _configService.GetConfigAsync();
        return Ok(config);
    }

    /// <summary>
    /// Получить конфигурацию по умолчанию
    /// </summary>
    [HttpGet("default")]
    public async Task<ActionResult<ConfigEntry>> GetDefaultConfig()
    {
        var config = await _configService.GetDefaultConfigAsync();
        return Ok(config);
    }

    /// <summary>
    /// Сохранить конфигурацию
    /// </summary>
    [HttpPost]
    public async Task<ActionResult> SaveConfig([FromBody] ConfigEntry config)
    {
        if (config == null)
            return BadRequest("Конфигурация не может быть пустой");

        await _configService.SaveConfigAsync(config);
        return Ok(new { message = "Конфигурация сохранена" });
    }
}

