using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Materials;
using LMS.Domain.Models.Enums;
using LMS.Domain.Services;
using MediatR;

namespace LMS.Application.Query.Material;

public record GetCourseMaterialsQuery(Guid CourseId, Guid RequesterId) : IRequest<IEnumerable<MaterialDto>>;
public record GetMaterialQuery(Guid MaterialId, Guid RequesterId) : IRequest<MaterialDto>;

public class GetCourseMaterialsHandler
    : IRequestHandler<GetCourseMaterialsQuery, IEnumerable<MaterialDto>>
{
    private readonly ISqlQueryService _db;
    public GetCourseMaterialsHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<MaterialDto>> Handle(GetCourseMaterialsQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT m.Id, m.TitleAr, m.TitleEn, m.DescriptionAr, m.DescriptionEn,
                   m.Type, m.ContentUrl, m.TextContent, m.[Order],
                   m.IsPublished, m.CourseId, m.CreatedAt
            FROM Materials m
            INNER JOIN Courses c ON c.Id = m.CourseId
            WHERE m.CourseId = @CourseId
              AND (
                    c.InstructorId = @RequesterId
                    OR (
                        m.IsPublished = 1
                        AND EXISTS (SELECT 1 FROM Enrollments e
                                    WHERE e.CourseId = m.CourseId
                                      AND e.StudentId = @RequesterId
                                      AND e.Status = 'Active')
                      )
                  )
            ORDER BY m.[Order]
            """;

        var rows = await _db.QueryAsync<Row>(sql, new { req.CourseId, req.RequesterId }, ct);
        return rows.Select(r => new MaterialDto(r.Id, r.TitleAr, r.TitleEn,
            r.DescriptionAr, r.DescriptionEn, Enum.Parse<MaterialType>(r.Type),
            r.ContentUrl, r.TextContent, r.Order, r.IsPublished, r.CourseId, r.CreatedAt));
    }

    private record Row(Guid Id, string TitleAr, string TitleEn, string? DescriptionAr,
        string? DescriptionEn, string Type, string ContentUrl, string? TextContent,
        int Order, bool IsPublished, Guid CourseId, DateTime CreatedAt);
}

public class GetMaterialHandler : IRequestHandler<GetMaterialQuery, MaterialDto>
{
    private readonly ISqlQueryService _db;
    public GetMaterialHandler(ISqlQueryService db) => _db = db;

    public async Task<MaterialDto> Handle(GetMaterialQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT m.Id, m.TitleAr, m.TitleEn, m.DescriptionAr, m.DescriptionEn,
                   m.Type, m.ContentUrl, m.TextContent, m.[Order],
                   m.IsPublished, m.CourseId, m.CreatedAt
            FROM Materials m WHERE m.Id = @MaterialId
            """;
        var row = await _db.QueryFirstOrDefaultAsync<Row>(sql, new { req.MaterialId }, ct)
            ?? throw new NotFoundException("Material", req.MaterialId);
        return new MaterialDto(row.Id, row.TitleAr, row.TitleEn,
            row.DescriptionAr, row.DescriptionEn, Enum.Parse<MaterialType>(row.Type),
            row.ContentUrl, row.TextContent, row.Order, row.IsPublished, row.CourseId, row.CreatedAt);
    }

    private record Row(Guid Id, string TitleAr, string TitleEn, string? DescriptionAr,
        string? DescriptionEn, string Type, string ContentUrl, string? TextContent,
        int Order, bool IsPublished, Guid CourseId, DateTime CreatedAt);
}
