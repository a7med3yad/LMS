
using LMS.Domain.Models;

namespace LMS.Domain.Repositories.Interfaces;

public interface IEnrollmentRepository : IRepository<Enrollment>
{
    Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId, CancellationToken ct = default);

    Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(Guid studentId, CancellationToken ct = default);

    Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseAsync(Guid courseId, CancellationToken ct = default);

    Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId, CancellationToken ct = default);

    Task<int> GetEnrollmentCountByCourseAsync(Guid courseId, CancellationToken ct = default);
}
