
namespace LMS.Application.DTOs.Exams;

public record StartExamResponseDto(
    Guid AttemptId,
    Guid ExamId,
    string TitleAr,
    string TitleEn,
    int DurationMinutes,
    DateTime StartedAt,
    DateTime ExpiresAt,
    IEnumerable<QuestionDto> Questions
);
