using LMS.DTOs.Users;
using LMS.Models.Enums;

namespace LMS.DTOs.Courses;

public record CourseDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    string? ThumbnailUrl,
    decimal Price,
    CourseStatus Status,
    UserSummaryDto Instructor,
    int EnrollmentCount,
    int MaterialCount,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
