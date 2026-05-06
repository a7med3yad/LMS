namespace LMS.Application.DTOs.Exams;

public record ExamDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    int DurationMinutes,
    int PassScore,
    int MaxAttempts,
    bool IsPublished,
    DateTime? AvailableFrom,
    DateTime? AvailableUntil,
    Guid CourseId,
    int QuestionCount,
    DateTime CreatedAt
);
