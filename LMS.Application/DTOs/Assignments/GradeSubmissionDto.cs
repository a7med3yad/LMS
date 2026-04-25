using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Assignments;

public record GradeSubmissionDto(
    [Required][Range(0, 1000)] int Grade,
    string? Feedback
);
