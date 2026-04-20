using System.ComponentModel.DataAnnotations;

namespace LMS.DTOs.Assignments;

public record GradeSubmissionDto(
    [Required][Range(0, 1000)] int Grade,
    string? Feedback
);
