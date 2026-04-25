using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Exams;

public record UpdateExamDto(
    [MaxLength(300)] string? TitleAr,
    [MaxLength(300)] string? TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    int? DurationMinutes,
    int? PassScore,
    int? MaxAttempts,
    bool? IsPublished,
    DateTime? AvailableFrom,
    DateTime? AvailableUntil
);
