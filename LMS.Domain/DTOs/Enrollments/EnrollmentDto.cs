using LMS.Domain.DTOs.Users;
using LMS.Domain.Models.Enums;

namespace LMS.Domain.DTOs.Enrollments;

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
