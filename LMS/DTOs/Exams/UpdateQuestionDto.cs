using System.ComponentModel.DataAnnotations;
using LMS.Models.Enums;

namespace LMS.DTOs.Exams;

public record UpdateQuestionDto(
    [MaxLength(2000)] string? TextAr,
    [MaxLength(2000)] string? TextEn,
    QuestionType? Type,
    int? Points,
    int? Order
);
