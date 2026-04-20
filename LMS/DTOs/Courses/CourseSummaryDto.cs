using LMS.Models.Enums;

namespace LMS.DTOs.Courses;

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
