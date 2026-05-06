using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Courses;

public record CreateCourseDto(
    [Required][MaxLength(300)] string TitleAr,
    [Required][MaxLength(300)] string TitleEn,
    [MaxLength(5000)] string? DescriptionAr,
    [MaxLength(5000)] string? DescriptionEn,
    string? ThumbnailUrl,
    [Range(0, double.MaxValue)] decimal Price
);
