using LMS.Application.Common.Exceptions;
using LMS.Domain.Services;
using LMS.Infrastructure.Dapper;
using MediatR;

namespace LMS.Application.Query.Payment;

public record GetCourseRevenueQuery(Guid CourseId, Guid RequesterId) : IRequest<decimal>;

public class GetCourseRevenueHandler : IRequestHandler<GetCourseRevenueQuery, decimal>
{
    private readonly ISqlQueryService _db;
    public GetCourseRevenueHandler(ISqlQueryService db) => _db = db;

    public async Task<decimal> Handle(GetCourseRevenueQuery req, CancellationToken ct)
    {
        var isOwner = await _db.QueryFirstOrDefaultAsync<int>(
            "SELECT COUNT(1) FROM Courses WHERE Id=@CourseId AND InstructorId=@RequesterId",
            new { req.CourseId, req.RequesterId }, ct);

        if (isOwner == 0) throw new ForbiddenException();

        return await _db.QueryFirstOrDefaultAsync<decimal>(
            "SELECT ISNULL(SUM(PaidAmount),0) FROM Enrollments WHERE CourseId=@CourseId",
            new { req.CourseId }, ct);
    }
}