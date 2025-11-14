namespace FileAnalyzer.Domain.Entities;

/// <summary>
/// Конфигурация для парсинга и форматирования логов.
/// </summary>
public class ConfigEntry
{
    /// <summary>
    /// Порядок полей в лог-записи (например, ["Дата", "Уровень", "Сообщение"]).
    /// </summary>
    public string[] FieldsOrder { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Разделитель полей в строке лога (например, " | ", "[]", "{}").
    /// </summary>
    public string Separator { get; set; } = string.Empty;

    /// <summary>
    /// Формат даты для парсинга и форматирования (например, "yyyy-MM-dd HH:mm:ss").
    /// </summary>
    public string DateFormat { get; set; } = string.Empty;
}

