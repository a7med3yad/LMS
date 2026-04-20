using LMS.Data;
using LMS.Entities;
using LMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories;

public class EnrollmentRepository : GenericRepository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(AppDbContext context) : base(context) { }

    public async Task<Enrollment?> GetByStudentAndCourseAsync(Guid studentId, Guid courseId, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(e => e.StudentId == studentId && e.CourseId == courseId, ct);

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByStudentAsync(Guid studentId, CancellationToken ct = default)
        => await _dbSet
            .Include(e => e.Course)
            .Include(e => e.Student)
            .Where(e => e.StudentId == studentId)
            .OrderByDescending(e => e.EnrolledAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<Enrollment>> GetEnrollmentsByCourseAsync(Guid courseId, CancellationToken ct = default)
        => await _dbSet
            .Include(e => e.Student)
            .Where(e => e.CourseId == courseId)
            .OrderByDescending(e => e.EnrolledAt)
            .ToListAsync(ct);

    public async Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId, CancellationToken ct = default)
        => await _dbSet.AnyAsync(e => e.StudentId == studentId && e.CourseId == courseId, ct);

    public async Task<int> GetEnrollmentCountByCourseAsync(Guid courseId, CancellationToken ct = default)
        => await _dbSet.CountAsync(e => e.CourseId == courseId, ct);
}
