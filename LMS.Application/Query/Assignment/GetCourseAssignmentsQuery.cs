using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Assignments;
using LMS.Domain.DTOs.Users;
using LMS.Domain.Models.Enums;
using LMS.Domain.Services;
using MediatR;

namespace LMS.Application.Query.Assignment;

public record GetCourseAssignmentsQuery(Guid CourseId, Guid RequesterId) : IRequest<IEnumerable<AssignmentDto>>;
public record GetAssignmentQuery(Guid AssignmentId, Guid RequesterId) : IRequest<AssignmentDto>;
public record GetMySubmissionQuery(Guid AssignmentId, Guid StudentId) : IRequest<AssignmentSubmissionDto>;
public record GetSubmissionsQuery(Guid AssignmentId, Guid InstructorId) : IRequest<IEnumerable<AssignmentSubmissionDto>>;

// ─── GetCourseAssignments ───
public class GetCourseAssignmentsHandler
    : IRequestHandler<GetCourseAssignmentsQuery, IEnumerable<AssignmentDto>>
{
    private readonly ISqlQueryService _db;
    public GetCourseAssignmentsHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<AssignmentDto>> Handle(GetCourseAssignmentsQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT a.Id, a.TitleAr, a.TitleEn, a.DescriptionAr, a.DescriptionEn,
                   a.SubmissionType, a.DeadLine, a.MaxGrade,
                   a.IsPublished, a.CourseId, a.CreatedAt
            FROM Assignments a
            INNER JOIN Courses c ON c.Id = a.CourseId
            WHERE a.CourseId = @CourseId
              AND (
                    c.InstructorId = @RequesterId
                    OR (
                        a.IsPublished = 1
                        AND EXISTS (SELECT 1 FROM Enrollments e
                                    WHERE e.CourseId = a.CourseId
                                      AND e.StudentId = @RequesterId)
                      )
                  )
            ORDER BY a.CreatedAt DESC
            """;

        return await _db.QueryAsync<AssignmentDto>(sql, new { req.CourseId, req.RequesterId }, ct);
    }
}

// ─── GetAssignment ───
public class GetAssignmentHandler : IRequestHandler<GetAssignmentQuery, AssignmentDto>
{
    private readonly ISqlQueryService _db;
    public GetAssignmentHandler(ISqlQueryService db) => _db = db;

    public async Task<AssignmentDto> Handle(GetAssignmentQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT a.Id, a.TitleAr, a.TitleEn, a.DescriptionAr, a.DescriptionEn,
                   a.SubmissionType, a.DeadLine, a.MaxGrade,
                   a.IsPublished, a.CourseId, a.CreatedAt
            FROM Assignments a WHERE a.Id = @AssignmentId
            """;
        return await _db.QueryFirstOrDefaultAsync<AssignmentDto>(sql, new { req.AssignmentId }, ct)
            ?? throw new NotFoundException("Assignment", req.AssignmentId);
    }
}

// ─── GetMySubmission ───
public class GetMySubmissionHandler : IRequestHandler<GetMySubmissionQuery, AssignmentSubmissionDto>
{
    private readonly ISqlQueryService _db;
    public GetMySubmissionHandler(ISqlQueryService db) => _db = db;

    public async Task<AssignmentSubmissionDto> Handle(GetMySubmissionQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT s.Id, s.AssignmentId, a.TitleEn AS AssignmentTitle,
                   CAST(u.Id AS NVARCHAR(36)) AS StudentId,
                   u.FullName AS StudentFullName, u.Email AS StudentEmail,
                   u.AvatarUrl AS StudentAvatarUrl, u.Role AS StudentRole,
                   s.TextAnswer, s.FileUrl, s.Grade, s.Feedback, s.SubmittedAt, s.GradedAt
            FROM AssignmentSubmissions s
            INNER JOIN Assignments a ON a.Id = s.AssignmentId
            INNER JOIN AspNetUsers u ON u.Id = s.StudentId
            WHERE s.AssignmentId = @AssignmentId AND s.StudentId = @StudentId
            """;
        var row = await _db.QueryFirstOrDefaultAsync<SubRow>(sql, new { req.AssignmentId, req.StudentId }, ct)
            ?? throw new NotFoundException("Submission not found.");
        return ToDto(row);
    }

    private static AssignmentSubmissionDto ToDto(SubRow r) => new(
        r.Id, r.AssignmentId, r.AssignmentTitle,
        new UserSummaryDto(r.StudentId, r.StudentFullName, r.StudentEmail, r.StudentAvatarUrl, r.StudentRole),
        r.TextAnswer, r.FileUrl, r.Grade, r.Feedback, r.SubmittedAt, r.GradedAt);

    private record SubRow(Guid Id, Guid AssignmentId, string AssignmentTitle,
        string StudentId, string StudentFullName, string StudentEmail,
        string? StudentAvatarUrl, string StudentRole,
        string? TextAnswer, string? FileUrl, int Grade, string? Feedback,
        DateTime SubmittedAt, DateTime? GradedAt);
}

// ─── GetSubmissions (instructor) ───
public class GetSubmissionsHandler : IRequestHandler<GetSubmissionsQuery, IEnumerable<AssignmentSubmissionDto>>
{
    private readonly ISqlQueryService _db;
    public GetSubmissionsHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<AssignmentSubmissionDto>> Handle(GetSubmissionsQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT s.Id, s.AssignmentId, a.TitleEn AS AssignmentTitle,
                   CAST(u.Id AS NVARCHAR(36)) AS StudentId,
                   u.FullName AS StudentFullName, u.Email AS StudentEmail,
                   u.AvatarUrl AS StudentAvatarUrl, u.Role AS StudentRole,
                   s.TextAnswer, s.FileUrl, s.Grade, s.Feedback, s.SubmittedAt, s.GradedAt
            FROM AssignmentSubmissions s
            INNER JOIN Assignments a ON a.Id = s.AssignmentId
            INNER JOIN Courses c ON c.Id = a.CourseId
            INNER JOIN AspNetUsers u ON u.Id = s.StudentId
            WHERE s.AssignmentId = @AssignmentId AND c.InstructorId = @InstructorId
            ORDER BY s.SubmittedAt DESC
            """;
        var rows = await _db.QueryAsync<SubRow>(sql, new { req.AssignmentId, req.InstructorId }, ct);
        return rows.Select(r => new AssignmentSubmissionDto(
            r.Id, r.AssignmentId, r.AssignmentTitle,
            new UserSummaryDto(r.StudentId, r.StudentFullName, r.StudentEmail, r.StudentAvatarUrl, r.StudentRole),
            r.TextAnswer, r.FileUrl, r.Grade, r.Feedback, r.SubmittedAt, r.GradedAt));
    }

    private record SubRow(Guid Id, Guid AssignmentId, string AssignmentTitle,
        string StudentId, string StudentFullName, string StudentEmail,
        string? StudentAvatarUrl, string StudentRole,
        string? TextAnswer, string? FileUrl, int Grade, string? Feedback,
        DateTime SubmittedAt, DateTime? GradedAt);
}
