using FileAnalyzer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileAnalyzer.Infrastructure.Data;

/// <summary>
/// Контекст базы данных для хранения логов
/// </summary>
public class LogAnalyzerDbContext : DbContext
{
    public LogAnalyzerDbContext(DbContextOptions<LogAnalyzerDbContext> options) 
        : base(options)
    {
    }

    /// <summary>
    /// Таблица логов
    /// </summary>
    public DbSet<LogEntry> Logs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<LogEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.Level).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Message).IsRequired();
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(500);
            entity.Property(e => e.CreatedAt).IsRequired();
            
            entity.HasIndex(e => e.Date);
            entity.HasIndex(e => e.Level);
            entity.HasIndex(e => e.FileName);
        });
    }
}

