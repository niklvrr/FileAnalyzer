using FileAnalyzer.Application.Abstractions;
using FileAnalyzer.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalyzer.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController(ILogService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<LogEntryDto>>> GetAll(CancellationToken ct) => Ok(await service.GetAllAsync(ct));

    [HttpGet("filter")]
    public async Task<ActionResult<IReadOnlyList<LogEntryDto>>> Filter([FromQuery] DateTime? from, [FromQuery] DateTime? to, [FromQuery] string? level, [FromQuery] string? keyword, CancellationToken ct)
        => Ok(await service.FilterAsync(from, to, level, keyword, ct));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<LogEntryDto?>> Get(Guid id, CancellationToken ct)
    {
        var item = await service.GetAsync(id, ct);
        return item is null ? NotFound() : Ok(item);
    }

    public record CreateLogRequest(DateTime Timestamp, string Level, string Message);

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateLogRequest req, CancellationToken ct)
    {
        var id = await service.CreateAsync(req.Timestamp, req.Level, req.Message, ct);
        return CreatedAtAction(nameof(Get), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateLogRequest req, CancellationToken ct)
    {
        await service.UpdateAsync(id, req.Timestamp, req.Level, req.Message, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await service.DeleteAsync(id, ct);
        return NoContent();
    }
}
