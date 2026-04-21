using System.ComponentModel.DataAnnotations;
using LMS.Domain.Models.Enums;

namespace LMS.Domain.DTOs.Assignments;

public record CreateAssignmentDto(
    [Required][MaxLength(300)] string TitleAr,
    [Required][MaxLength(300)] string TitleEn,
    [Required] string DescriptionAr,
    [Required] string DescriptionEn,
    [Required] SubmissionType SubmissionType,
    [Required] DateTime DeadLine,
    [Range(1, 1000)] int MaxGrade
);
