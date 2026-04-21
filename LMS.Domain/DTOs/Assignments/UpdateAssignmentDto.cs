using LMS.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Assignments;

public record UpdateAssignmentDto(
    [MaxLength(300)] string? TitleAr,
    [MaxLength(300)] string? TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    SubmissionType? SubmissionType,
    DateTime? DeadLine,
    int? MaxGrade,
    bool? IsPublished
);
