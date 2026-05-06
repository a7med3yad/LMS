using LMS.Application.DTOs.Payments;
using LMS.Domain.Services;
using MediatR;

namespace LMS.Application.Query.Payment;

public record GetMyPaymentsQuery(Guid StudentId) : IRequest<IEnumerable<PaymentDto>>;

public class GetMyPaymentsHandler : IRequestHandler<GetMyPaymentsQuery, IEnumerable<PaymentDto>>
{
    private readonly ISqlQueryService _db;
    public GetMyPaymentsHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<PaymentDto>> Handle(GetMyPaymentsQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT p.Id, p.StudentId, p.CourseId,
                   p.Amount, p.Status, p.Provider,
                   p.ProviderTransactionId,
                   p.CreatedAt, p.PaidAt
            FROM Payments p
            WHERE p.StudentId = @StudentId
            ORDER BY p.CreatedAt DESC
            """;

        return await _db.QueryAsync<PaymentDto>(sql, new { req.StudentId }, ct);
    }
}
