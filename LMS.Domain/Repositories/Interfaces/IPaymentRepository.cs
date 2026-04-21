using LMS.Domain.Models;

namespace LMS.Domain.Repositories.Interfaces;

public interface IPaymentRepository : IRepository<Enrollment>
{
    Task<IEnumerable<Enrollment>> GetPaidEnrollmentsAsync(Guid courseId, CancellationToken ct = default);

    Task<decimal> GetTotalRevenueForCourseAsync(Guid courseId, CancellationToken ct = default);

    Task<decimal> GetTotalRevenueForInstructorAsync(Guid instructorId, CancellationToken ct = default);
}
