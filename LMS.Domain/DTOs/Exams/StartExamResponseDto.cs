
namespace LMS.Domain.DTOs.Exams;

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
