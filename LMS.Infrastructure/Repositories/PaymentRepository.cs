using LMS.Domain.Models;
using LMS.Domain.Repositories.Interfaces;
using LMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class PaymentRepository : GenericRepository<Enrollment>, IPaymentRepository
{
    public PaymentRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Enrollment>> GetPaidEnrollmentsAsync(Guid courseId, CancellationToken ct = default)
        => await _dbSet
            .Include(e => e.Student)
            .Where(e => e.CourseId == courseId && e.PaidAmount > 0)
            .OrderByDescending(e => e.EnrolledAt)
            .ToListAsync(ct);

    public async Task<decimal> GetTotalRevenueForCourseAsync(Guid courseId, CancellationToken ct = default)
        => await _dbSet
            .Where(e => e.CourseId == courseId)
            .SumAsync(e => e.PaidAmount, ct);

    public async Task<decimal> GetTotalRevenueForInstructorAsync(Guid instructorId, CancellationToken ct = default)
        => await _dbSet
            .Include(e => e.Course)
            .Where(e => e.Course.InstructorId == instructorId)
            .SumAsync(e => e.PaidAmount, ct);
}
