using System.ComponentModel.DataAnnotations;
using LMS.Models.Enums;

namespace LMS.DTOs.Exams;

public record CreateQuestionDto(
    [Required][MaxLength(2000)] string TextAr,
    [Required][MaxLength(2000)] string TextEn,
    [Required] QuestionType Type,
    [Range(1, 100)] int Points,
    int Order,
    IEnumerable<CreateQuestionChoiceDto>? Choices
);

public record CreateQuestionChoiceDto(
    [Required][MaxLength(1000)] string TextAr,
    [Required][MaxLength(1000)] string TextEn,
    bool IsCorrect
);
