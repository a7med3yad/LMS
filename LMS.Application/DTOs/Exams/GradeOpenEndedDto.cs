using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Exams;

public record GradeOpenEndedDto(
    [Required] Guid AnswerId,
    [Required][Range(0, 100)] int Grade
);
