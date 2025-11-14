namespace FileAnalyzer.Domain.Interfaces;

using FileAnalyzer.Domain.Entities;

public interface ILogRepository
{
    Task<LogEntry?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<LogEntry>> GetAllAsync(CancellationToken ct = default);
    Task<IReadOnlyList<LogEntry>> FilterAsync(DateTime? from, DateTime? to, string? level, string? keyword, CancellationToken ct = default);
    Task AddAsync(LogEntry entry, CancellationToken ct = default);
    Task UpdateAsync(LogEntry entry, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
