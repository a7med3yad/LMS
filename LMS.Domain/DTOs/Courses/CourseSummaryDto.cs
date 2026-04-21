using LMS.Domain.Models.Enums;

namespace LMS.Domain.DTOs.Courses;

public record CourseSummaryDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string? ThumbnailUrl,
    decimal Price,
    CourseStatus Status,
    string InstructorName,
    int EnrollmentCount,
    DateTime CreatedAt
);
