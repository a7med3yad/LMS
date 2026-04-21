using LMS.Domain.DTOs.Users;
using LMS.Domain.Models.Enums;

namespace LMS.Domain.DTOs.Courses;

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
