using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Vouchers;
using LMS.Domain.Services;
using LMS.Infrastructure.Dapper;
using MediatR;

namespace LMS.Application.Query.Voucher;

public record GetCourseVouchersQuery(Guid CourseId, Guid RequesterId) : IRequest<IEnumerable<VoucherDto>>;
public record ValidateVoucherQuery(string Code, Guid CourseId) : IRequest<VoucherDto>;

public class GetCourseVouchersHandler : IRequestHandler<GetCourseVouchersQuery, IEnumerable<VoucherDto>>
{
    private readonly ISqlQueryService _db;
    public GetCourseVouchersHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<VoucherDto>> Handle(GetCourseVouchersQuery req, CancellationToken ct)
    {
        var own = await _db.QueryFirstOrDefaultAsync<int>(
            "SELECT COUNT(1) FROM Courses WHERE Id=@CourseId AND InstructorId=@RequesterId",
            new { req.CourseId, req.RequesterId }, ct);
        if (own == 0) throw new ForbiddenException();

        const string sql = """
            SELECT v.Id,v.Code,v.CourseId,c.TitleEn AS CourseTitleEn,v.DiscountPercent,
                   v.DiscountAmount,v.MaxUses,v.UsedCount,v.ExpiresAt,v.IsActive,v.CreatedAt
            FROM Vouchers v INNER JOIN Courses c ON c.Id=v.CourseId
            WHERE v.CourseId=@CourseId ORDER BY v.CreatedAt DESC
            """;
        return await _db.QueryAsync<VoucherDto>(sql, new { req.CourseId }, ct);
    }
}

public class ValidateVoucherHandler : IRequestHandler<ValidateVoucherQuery, VoucherDto>
{
    private readonly ISqlQueryService _db;
    public ValidateVoucherHandler(ISqlQueryService db) => _db = db;

    public async Task<VoucherDto> Handle(ValidateVoucherQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT v.Id,v.Code,v.CourseId,c.TitleEn AS CourseTitleEn,v.DiscountPercent,
                   v.DiscountAmount,v.MaxUses,v.UsedCount,v.ExpiresAt,v.IsActive,v.CreatedAt
            FROM Vouchers v INNER JOIN Courses c ON c.Id=v.CourseId
            WHERE v.Code=@Code AND v.CourseId=@CourseId AND v.IsActive=1
              AND v.ExpiresAt > GETUTCDATE() AND v.UsedCount < v.MaxUses
            """;
        return await _db.QueryFirstOrDefaultAsync<VoucherDto>(
            sql, new { req.Code, req.CourseId }, ct)
            ?? throw new NotFoundException("Voucher is invalid or expired.");
    }
}