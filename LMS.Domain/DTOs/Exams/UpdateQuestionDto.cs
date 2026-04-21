using LMS.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Exams;

public record UpdateQuestionDto(
    [MaxLength(2000)] string? TextAr,
    [MaxLength(2000)] string? TextEn,
    QuestionType? Type,
    int? Points,
    int? Order
);
