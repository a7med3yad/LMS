using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Exams;

public record CreateExamDto(
    [Required][MaxLength(300)] string TitleAr,
    [Required][MaxLength(300)] string TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    [Required][Range(1, 600)] int DurationMinutes,
    [Required][Range(0, 100)] int PassScore,
    [Range(1, 10)] int MaxAttempts,
    DateTime? AvailableFrom,
    DateTime? AvailableUntil
);
