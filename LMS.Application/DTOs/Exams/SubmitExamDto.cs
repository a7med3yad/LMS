using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Exams;

public record ExamAnswerDto(
    Guid QuestionId,
    Guid? SelectedChoiceId,
    string? OpenAnswer
);

public record SubmitExamDto(
    [Required] Guid AttemptId,
    [Required] IEnumerable<ExamAnswerDto> Answers
);

public record ExamAnswerResultDto(
    Guid QuestionId,
    string QuestionText,
    Guid? SelectedChoiceId,
    string? OpenAnswer,
    bool? IsCorrect,
    int? ManualGrade
);
