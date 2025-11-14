using FileAnalyzer.Application.Interfaces;
using FileAnalyzer.Application.Services;
using FileAnalyzer.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalyzer.API.Controllers;

/// <summary>
/// Контроллер для работы с логами
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
    private readonly ILogRepository _logRepository;
    private readonly ILogParser _logParser;
    private readonly LogFilterService _filterService;
    private readonly LogSorterService _sorterService;
    private readonly LogStatisticsService _statisticsService;

    public LogsController(
        ILogRepository logRepository,
        ILogParser logParser,
        LogFilterService filterService,
        LogSorterService sorterService,
        LogStatisticsService statisticsService)
    {
        _logRepository = logRepository;
        _logParser = logParser;
        _filterService = filterService;
        _sorterService = sorterService;
        _statisticsService = statisticsService;
    }

    /// <summary>
    /// Получить все логи
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogEntry>>> GetAll()
    {
        var logs = await _logRepository.GetAllAsync();
        return Ok(logs);
    }

    /// <summary>
    /// Получить лог по ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<LogEntry>> GetById(Guid id)
    {
        var log = await _logRepository.GetByIdAsync(id);
        if (log == null)
            return NotFound();
            
        return Ok(log);
    }

    /// <summary>
    /// Загрузить логи из файлов
    /// </summary>
    [HttpPost("upload")]
    public async Task<ActionResult> UploadFromFiles([FromBody] List<string> filePaths)
    {
        if (filePaths == null || filePaths.Count == 0)
            return BadRequest("Пути к файлам не указаны");

        var logs = await _logParser.ParseFromFilesAsync(filePaths);
        if (logs == null || logs.Count == 0)
            return BadRequest("Не удалось прочитать логи из файлов");

        await _logRepository.AddRangeAsync(logs);
        return Ok(new { message = $"Добавлено {logs.Count} записей", count = logs.Count });
    }

    /// <summary>
    /// Получить отфильтрованные логи
    /// </summary>
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<LogEntry>>> GetFiltered(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? level,
        [FromQuery] string? messageKeyword)
    {
        var logs = await _logRepository.GetFilteredAsync(startDate, endDate, level, messageKeyword);
        return Ok(logs);
    }

    /// <summary>
    /// Получить отсортированные логи
    /// </summary>
    [HttpGet("sorted")]
    public async Task<ActionResult<IEnumerable<LogEntry>>> GetSorted(
        [FromQuery] string field = "date",
        [FromQuery] bool ascending = false)
    {
        var logs = (await _logRepository.GetAllAsync()).ToList();
        
        var sorted = ascending 
            ? _sorterService.AscendingSort(logs, field)
            : _sorterService.DescendingSort(logs, field);
            
        return Ok(sorted);
    }

    /// <summary>
    /// Получить статистику по логам
    /// </summary>
    [HttpGet("statistics")]
    public async Task<ActionResult> GetStatistics()
    {
        var logs = (await _logRepository.GetAllAsync()).ToList();
        var stats = _statisticsService.GetStatistics(logs);
        return Ok(stats);
    }

    /// <summary>
    /// Удалить лог по ID
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _logRepository.DeleteAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Очистить все логи
    /// </summary>
    [HttpDelete]
    public async Task<ActionResult> ClearAll()
    {
        await _logRepository.ClearAllAsync();
        return NoContent();
    }
}

