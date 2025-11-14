namespace FileAnalyzer.Application.Abstractions;

using FileAnalyzer.Application.DTOs;

public interface ILogService
{
    Task<LogEntryDto?> GetAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<LogEntryDto>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<LogEntryDto>> FilterAsync(DateTime? from, DateTime? to, string? level, string? keyword, CancellationToken ct = default);
    Task<Guid> CreateAsync(DateTime timestamp, string level, string message, CancellationToken ct = default);
    Task UpdateAsync(Guid id, DateTime timestamp, string level, string message, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
