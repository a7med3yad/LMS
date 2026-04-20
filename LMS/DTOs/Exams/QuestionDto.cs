using LMS.Models.Enums;

namespace LMS.DTOs.Exams;

public record QuestionChoiceDto(
    Guid Id,
    string TextAr,
    string TextEn,
    bool? IsCorrect
);

public record QuestionDto(
    Guid Id,
    string TextAr,
    string TextEn,
    QuestionType Type,
    int Points,
    int Order,
    IEnumerable<QuestionChoiceDto> Choices
);
