
namespace LMS.Domain.DTOs.Exams;

public record ExamAttemptDto(
    Guid Id,
    Guid ExamId,
    string ExamTitle,
    int Score,
    bool IsPassed,
    DateTime StartedAt,
    DateTime? SubmittedAt,
    IEnumerable<ExamAnswerResultDto>? Answers
);
