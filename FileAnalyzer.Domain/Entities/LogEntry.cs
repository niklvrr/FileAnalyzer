namespace FileAnalyzer.Domain.Entities;

/// <summary>
/// Сущность лог-записи (Entity).
/// Содержит информацию о дате, уровне важности, сообщении и имени файла.
/// </summary>
public class LogEntry
{
    /// <summary>
    /// Уникальный идентификатор лог-записи
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Дата и время создания лог-записи.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Уровень важности лог-записи (например, Info, Warning, Error).
    /// </summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Сообщение, содержащее подробности лог-записи.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Имя файла, из которого была считана лог-запись.
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания записи в системе
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
