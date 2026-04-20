using LMS.DTOs.Users;
using LMS.Models.Enums;

namespace LMS.DTOs.Enrollments;

public record EnrollmentDto(
    Guid Id,
    Guid CourseId,
    string CourseTitleAr,
    string CourseTitleEn,
    UserSummaryDto Student,
    EnrollmentStatus Status,
    decimal PaidAmount,
    string? VoucherCode,
    DateTime EnrolledAt,
    DateTime? CompletedAt
);
