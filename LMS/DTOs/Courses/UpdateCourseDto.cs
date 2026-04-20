using System.ComponentModel.DataAnnotations;
using LMS.Models.Enums;

namespace LMS.DTOs.Courses;

public record UpdateCourseDto(
    [MaxLength(300)] string? TitleAr,
    [MaxLength(300)] string? TitleEn,
    [MaxLength(5000)] string? DescriptionAr,
    [MaxLength(5000)] string? DescriptionEn,
    string? ThumbnailUrl,
    decimal? Price,
    CourseStatus? Status
);
