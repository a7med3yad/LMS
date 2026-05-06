using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Enrollments;
using LMS.Application.DTOs.Users;
using LMS.Domain.Models.Enums;
using LMS.Domain.Services;
using MediatR;

namespace LMS.Application.Query.Enrollment;

public record GetMyEnrollmentsQuery(Guid StudentId) : IRequest<IEnumerable<EnrollmentDto>>;
public record GetEnrollmentQuery(Guid EnrollmentId, Guid RequesterId) : IRequest<EnrollmentDto>;
public record GetCourseEnrollmentsQuery(Guid CourseId, Guid RequesterId) : IRequest<IEnumerable<EnrollmentDto>>;

// ─── GetMyEnrollments ───
public class GetMyEnrollmentsHandler : IRequestHandler<GetMyEnrollmentsQuery, IEnumerable<EnrollmentDto>>
{
    private readonly ISqlQueryService _db;
    public GetMyEnrollmentsHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<EnrollmentDto>> Handle(GetMyEnrollmentsQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT e.Id, e.CourseId, c.TitleAr AS CourseTitleAr, c.TitleEn AS CourseTitleEn,
                   CAST(u.Id AS NVARCHAR(36)) AS StudentId, u.FullName AS StudentFullName,
                   u.Email AS StudentEmail, u.AvatarUrl AS StudentAvatarUrl, u.Role AS StudentRole,
                   e.Status, e.PaidAmount, v.Code AS VoucherCode, e.EnrolledAt, e.CompletedAt
            FROM Enrollments e
            INNER JOIN Courses c ON c.Id = e.CourseId
            INNER JOIN AspNetUsers u ON u.Id = e.StudentId
            LEFT  JOIN Vouchers v ON v.Id = e.VoucherId
            WHERE e.StudentId = @StudentId ORDER BY e.EnrolledAt DESC
            """;

        var rows = await _db.QueryAsync<Row>(sql, new { req.StudentId }, ct);
        return rows.Select(ToDto);
    }

    private static EnrollmentDto ToDto(Row r) => new(
        r.Id, r.CourseId, r.CourseTitleAr, r.CourseTitleEn,
        new UserSummaryDto(r.StudentId, r.StudentFullName, r.StudentEmail, r.StudentAvatarUrl, r.StudentRole),
        Enum.Parse<EnrollmentStatus>(r.Status), r.PaidAmount, r.VoucherCode, r.EnrolledAt, r.CompletedAt);

    private record Row(Guid Id, Guid CourseId, string CourseTitleAr, string CourseTitleEn,
        string StudentId, string StudentFullName, string StudentEmail, string? StudentAvatarUrl,
        string StudentRole, string Status, decimal PaidAmount, string? VoucherCode,
        DateTime EnrolledAt, DateTime? CompletedAt);
}

// ─── GetEnrollment ───
public class GetEnrollmentHandler : IRequestHandler<GetEnrollmentQuery, EnrollmentDto>
{
    private readonly ISqlQueryService _db;
    public GetEnrollmentHandler(ISqlQueryService db) => _db = db;

    public async Task<EnrollmentDto> Handle(GetEnrollmentQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT e.Id, e.CourseId, c.TitleAr AS CourseTitleAr, c.TitleEn AS CourseTitleEn,
                   CAST(u.Id AS NVARCHAR(36)) AS StudentId, u.FullName AS StudentFullName,
                   u.Email AS StudentEmail, u.AvatarUrl AS StudentAvatarUrl, u.Role AS StudentRole,
                   e.Status, e.PaidAmount, v.Code AS VoucherCode, e.EnrolledAt, e.CompletedAt
            FROM Enrollments e
            INNER JOIN Courses c ON c.Id = e.CourseId
            INNER JOIN AspNetUsers u ON u.Id = e.StudentId
            LEFT  JOIN Vouchers v ON v.Id = e.VoucherId
            WHERE e.Id = @EnrollmentId
            """;
        var row = await _db.QueryFirstOrDefaultAsync<Row>(sql, new { req.EnrollmentId }, ct)
            ?? throw new NotFoundException("Enrollment", req.EnrollmentId);
        return new EnrollmentDto(
            row.Id, row.CourseId, row.CourseTitleAr, row.CourseTitleEn,
            new UserSummaryDto(row.StudentId, row.StudentFullName, row.StudentEmail, row.StudentAvatarUrl, row.StudentRole),
            Enum.Parse<EnrollmentStatus>(row.Status), row.PaidAmount, row.VoucherCode, row.EnrolledAt, row.CompletedAt);
    }

    private record Row(Guid Id, Guid CourseId, string CourseTitleAr, string CourseTitleEn,
        string StudentId, string StudentFullName, string StudentEmail, string? StudentAvatarUrl,
        string StudentRole, string Status, decimal PaidAmount, string? VoucherCode,
        DateTime EnrolledAt, DateTime? CompletedAt);
}

// ─── GetCourseEnrollments ───
public class GetCourseEnrollmentsHandler : IRequestHandler<GetCourseEnrollmentsQuery, IEnumerable<EnrollmentDto>>
{
    private readonly ISqlQueryService _db;
    public GetCourseEnrollmentsHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<EnrollmentDto>> Handle(GetCourseEnrollmentsQuery req, CancellationToken ct)
    {
        var own = await _db.QueryFirstOrDefaultAsync<int>(
            "SELECT COUNT(1) FROM Courses WHERE Id=@CourseId AND InstructorId=@RequesterId",
            new { req.CourseId, req.RequesterId }, ct);
        if (own == 0) throw new ForbiddenException();

        const string sql = """
            SELECT e.Id, e.CourseId, c.TitleAr AS CourseTitleAr, c.TitleEn AS CourseTitleEn,
                   CAST(u.Id AS NVARCHAR(36)) AS StudentId, u.FullName AS StudentFullName,
                   u.Email AS StudentEmail, u.AvatarUrl AS StudentAvatarUrl, u.Role AS StudentRole,
                   e.Status, e.PaidAmount, v.Code AS VoucherCode, e.EnrolledAt, e.CompletedAt
            FROM Enrollments e
            INNER JOIN Courses c ON c.Id = e.CourseId
            INNER JOIN AspNetUsers u ON u.Id = e.StudentId
            LEFT  JOIN Vouchers v ON v.Id = e.VoucherId
            WHERE e.CourseId = @CourseId ORDER BY e.EnrolledAt DESC
            """;
        var rows = await _db.QueryAsync<Row>(sql, new { req.CourseId }, ct);
        return rows.Select(r => new EnrollmentDto(
            r.Id, r.CourseId, r.CourseTitleAr, r.CourseTitleEn,
            new UserSummaryDto(r.StudentId, r.StudentFullName, r.StudentEmail, r.StudentAvatarUrl, r.StudentRole),
            Enum.Parse<EnrollmentStatus>(r.Status), r.PaidAmount, r.VoucherCode, r.EnrolledAt, r.CompletedAt));
    }

    private record Row(Guid Id, Guid CourseId, string CourseTitleAr, string CourseTitleEn,
        string StudentId, string StudentFullName, string StudentEmail, string? StudentAvatarUrl,
        string StudentRole, string Status, decimal PaidAmount, string? VoucherCode,
        DateTime EnrolledAt, DateTime? CompletedAt);
}