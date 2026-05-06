using Dapper;
using LMS.Application.Common.Exceptions;
using LMS.Application.Common.Pagination;
using LMS.Application.DTOs.Courses;
using LMS.Domain.DTOs.Users;
using LMS.Domain.Models.Enums;
using LMS.Domain.Services;
using LMS.Infrastructure.Dapper;
using MediatR;

namespace LMS.Application.Query.Course;

public record GetCoursesQuery(CourseFilterDto Filter) : IRequest<PagedResult<CourseSummaryDto>>;
public record GetCourseQuery(Guid CourseId) : IRequest<CourseDto>;
public record GetMyCoursesQuery(Guid InstructorId) : IRequest<IEnumerable<CourseSummaryDto>>;
public record GetEnrolledCoursesQuery(Guid StudentId) : IRequest<IEnumerable<CourseSummaryDto>>;

// ─── GetCourses (paged) ───
public class GetCoursesHandler : IRequestHandler<GetCoursesQuery, PagedResult<CourseSummaryDto>>
{
    private readonly ISqlQueryService _db;
    public GetCoursesHandler(ISqlQueryService db) => _db = db;

    public async Task<PagedResult<CourseSummaryDto>> Handle(GetCoursesQuery req, CancellationToken ct)
    {
        var f = req.Filter;
        int offset = (f.Page - 1) * f.PageSize;
        var where = new List<string> { "1=1" };
        var param = new DynamicParameters();

        if (f.Status.HasValue) { where.Add("c.Status = @Status"); param.Add("Status", f.Status.Value.ToString()); }
        if (f.InstructorId.HasValue) { where.Add("c.InstructorId = @InstructorId"); param.Add("InstructorId", f.InstructorId.Value); }
        if (f.MinPrice.HasValue) { where.Add("c.Price >= @MinPrice"); param.Add("MinPrice", f.MinPrice.Value); }
        if (f.MaxPrice.HasValue) { where.Add("c.Price <= @MaxPrice"); param.Add("MaxPrice", f.MaxPrice.Value); }
        if (!string.IsNullOrWhiteSpace(f.Search))
        {
            where.Add("(c.TitleAr LIKE @Search OR c.TitleEn LIKE @Search)");
            param.Add("Search", $"%{f.Search}%");
        }

        var w = string.Join(" AND ", where);
        var sort = f.SortBy?.ToLower() switch
        {
            "price" => "c.Price",
            "createdat" => "c.CreatedAt",
            "title" => "c.TitleEn",
            _ => "c.CreatedAt"
        };
        var dir = f.SortDescending ? "DESC" : "ASC";
        param.Add("Offset", offset); param.Add("PageSize", f.PageSize);

        var dataSql = $"""
            SELECT c.Id, c.TitleAr, c.TitleEn, c.ThumbnailUrl, c.Price, c.Status,
                   c.CreatedAt, u.FullName AS InstructorName,
                   (SELECT COUNT(*) FROM Enrollments e WHERE e.CourseId = c.Id) AS EnrollmentCount
            FROM Courses c
            INNER JOIN AspNetUsers u ON u.Id = c.InstructorId
            WHERE {w}
            ORDER BY {sort} {dir}
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            """;

        var countSql = $"SELECT COUNT(*) FROM Courses c WHERE {w}";

        var (rows, total) = await _db.QueryPagedAsync<SummaryRow>(dataSql, countSql, param, ct);

        return new PagedResult<CourseSummaryDto>
        {
            Items = rows.Select(r => new CourseSummaryDto(r.Id, r.TitleAr, r.TitleEn,
                             r.ThumbnailUrl, r.Price,
                             Enum.Parse<CourseStatus>(r.Status),
                             r.InstructorName, r.EnrollmentCount, r.CreatedAt)),
            TotalCount = total,
            Page = f.Page,
            PageSize = f.PageSize
        };
    }

    private record SummaryRow(Guid Id, string TitleAr, string TitleEn, string? ThumbnailUrl,
        decimal Price, string Status, DateTime CreatedAt, string InstructorName, int EnrollmentCount);
}

// ─── GetCourse (single) ───
public class GetCourseHandler : IRequestHandler<GetCourseQuery, CourseDto>
{
    private readonly ISqlQueryService _db;
    public GetCourseHandler(ISqlQueryService db) => _db = db;

    public async Task<CourseDto> Handle(GetCourseQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT c.Id, c.TitleAr, c.TitleEn, c.DescriptionAr, c.DescriptionEn,
                   c.ThumbnailUrl, c.Price, c.Status, c.CreatedAt, c.UpdatedAt,
                   CAST(u.Id AS NVARCHAR(36)) AS InstructorId,
                   u.FullName AS InstructorFullName, u.Email AS InstructorEmail,
                   u.AvatarUrl AS InstructorAvatarUrl, u.Role AS InstructorRole,
                   (SELECT COUNT(*) FROM Enrollments e WHERE e.CourseId = c.Id) AS EnrollmentCount,
                   (SELECT COUNT(*) FROM Materials   m WHERE m.CourseId = c.Id) AS MaterialCount
            FROM Courses c
            INNER JOIN AspNetUsers u ON u.Id = c.InstructorId
            WHERE c.Id = @CourseId
            """;
        var row = await _db.QueryFirstOrDefaultAsync<DetailRow>(sql, new { req.CourseId }, ct)
            ?? throw new NotFoundException("Course", req.CourseId);

        return new CourseDto(row.Id, row.TitleAr, row.TitleEn, row.DescriptionAr, row.DescriptionEn,
            row.ThumbnailUrl, row.Price, Enum.Parse<CourseStatus>(row.Status),
            new UserSummaryDto(row.InstructorId, row.InstructorFullName, row.InstructorEmail,
                row.InstructorAvatarUrl, row.InstructorRole),
            row.EnrollmentCount, row.MaterialCount, row.CreatedAt, row.UpdatedAt);
    }

    private record DetailRow(Guid Id, string TitleAr, string TitleEn, string? DescriptionAr,
        string? DescriptionEn, string? ThumbnailUrl, decimal Price, string Status,
        DateTime CreatedAt, DateTime UpdatedAt, string InstructorId, string InstructorFullName,
        string InstructorEmail, string? InstructorAvatarUrl, string InstructorRole,
        int EnrollmentCount, int MaterialCount);
}

// ─── GetMyCourses (instructor) ───
public class GetMyCoursesHandler : IRequestHandler<GetMyCoursesQuery, IEnumerable<CourseSummaryDto>>
{
    private readonly ISqlQueryService _db;
    public GetMyCoursesHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<CourseSummaryDto>> Handle(GetMyCoursesQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT c.Id, c.TitleAr, c.TitleEn, c.ThumbnailUrl, c.Price, c.Status,
                   c.CreatedAt, u.FullName AS InstructorName,
                   (SELECT COUNT(*) FROM Enrollments e WHERE e.CourseId = c.Id) AS EnrollmentCount
            FROM Courses c INNER JOIN AspNetUsers u ON u.Id = c.InstructorId
            WHERE c.InstructorId = @InstructorId ORDER BY c.CreatedAt DESC
            """;
        var rows = await _db.QueryAsync<SRow>(sql, new { req.InstructorId }, ct);
        return rows.Select(r => new CourseSummaryDto(r.Id, r.TitleAr, r.TitleEn,
            r.ThumbnailUrl, r.Price, Enum.Parse<CourseStatus>(r.Status),
            r.InstructorName, r.EnrollmentCount, r.CreatedAt));
    }

    private record SRow(Guid Id, string TitleAr, string TitleEn, string? ThumbnailUrl,
        decimal Price, string Status, DateTime CreatedAt, string InstructorName, int EnrollmentCount);
}

// ─── GetEnrolledCourses (student) ───
public class GetEnrolledCoursesHandler : IRequestHandler<GetEnrolledCoursesQuery, IEnumerable<CourseSummaryDto>>
{
    private readonly ISqlQueryService _db;
    public GetEnrolledCoursesHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<CourseSummaryDto>> Handle(GetEnrolledCoursesQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT c.Id, c.TitleAr, c.TitleEn, c.ThumbnailUrl, c.Price, c.Status,
                   c.CreatedAt, u.FullName AS InstructorName,
                   (SELECT COUNT(*) FROM Enrollments e2 WHERE e2.CourseId = c.Id) AS EnrollmentCount
            FROM Enrollments e
            INNER JOIN Courses c ON c.Id = e.CourseId
            INNER JOIN AspNetUsers u ON u.Id = c.InstructorId
            WHERE e.StudentId = @StudentId ORDER BY e.EnrolledAt DESC
            """;
        var rows = await _db.QueryAsync<SRow>(sql, new { req.StudentId }, ct);
        return rows.Select(r => new CourseSummaryDto(r.Id, r.TitleAr, r.TitleEn,
            r.ThumbnailUrl, r.Price, Enum.Parse<CourseStatus>(r.Status),
            r.InstructorName, r.EnrollmentCount, r.CreatedAt));
    }

    private record SRow(Guid Id, string TitleAr, string TitleEn, string? ThumbnailUrl,
        decimal Price, string Status, DateTime CreatedAt, string InstructorName, int EnrollmentCount);
}