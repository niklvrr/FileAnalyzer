using FileAnalyzer.Application.Interfaces;
using FileAnalyzer.Domain.Entities;
using FileAnalyzer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FileAnalyzer.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с логами в базе данных SQLite
/// </summary>
public class LogRepository : ILogRepository
{
    private readonly LogAnalyzerDbContext _context;

    public LogRepository(LogAnalyzerDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LogEntry>> GetAllAsync()
    {
        return await _context.Logs
            .OrderByDescending(l => l.Date)
            .ToListAsync();
    }

    public async Task<LogEntry?> GetByIdAsync(Guid id)
    {
        return await _context.Logs.FindAsync(id);
    }

    public async Task AddRangeAsync(IEnumerable<LogEntry> logs)
    {
        await _context.Logs.AddRangeAsync(logs);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var log = await _context.Logs.FindAsync(id);
        if (log != null)
        {
            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();
        }
    }

    public async Task ClearAllAsync()
    {
        _context.Logs.RemoveRange(_context.Logs);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<LogEntry>> GetFilteredAsync(
        DateTime? startDate, 
        DateTime? endDate, 
        string? level, 
        string? messageKeyword)
    {
        var query = _context.Logs.AsQueryable();

        if (startDate.HasValue)
        {
            query = query.Where(l => l.Date >= startDate.Value);
        }

        if (endDate.HasValue)
        {
            query = query.Where(l => l.Date <= endDate.Value);
        }

        if (!string.IsNullOrWhiteSpace(level))
        {
            query = query.Where(l => l.Level == level);
        }

        if (!string.IsNullOrWhiteSpace(messageKeyword))
        {
            query = query.Where(l => l.Message.Contains(messageKeyword));
        }

        return await query.OrderByDescending(l => l.Date).ToListAsync();
    }
}

