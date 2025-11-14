namespace FileAnalyzer.Application.DTOs;

public record LogEntryDto(Guid Id, DateTime Timestamp, string Level, string Message);
