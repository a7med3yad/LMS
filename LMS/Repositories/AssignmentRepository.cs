using LMS.Data;
using LMS.Entities;
using LMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories;

public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
{
    public AssignmentRepository(AppDbContext context) : base(context) { }

    public async Task<Assignment?> GetAssignmentWithSubmissionsAsync(Guid assignmentId, CancellationToken ct = default)
        => await _dbSet
            .Include(a => a.Submissions)
                .ThenInclude(s => s.Student)
            .FirstOrDefaultAsync(a => a.Id == assignmentId, ct);

    public async Task<IEnumerable<Assignment>> GetAssignmentsByCourseAsync(Guid courseId, CancellationToken ct = default)
        => await _dbSet
            .Where(a => a.CourseId == courseId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);

    public async Task<AssignmentSubmission?> GetSubmissionByStudentAsync(Guid assignmentId, Guid studentId, CancellationToken ct = default)
        => await _context.AssignmentSubmissions
            .Include(s => s.Student)
            .FirstOrDefaultAsync(s => s.AssignmentId == assignmentId && s.StudentId == studentId, ct);

    public async Task<IEnumerable<AssignmentSubmission>> GetSubmissionsByAssignmentAsync(Guid assignmentId, CancellationToken ct = default)
        => await _context.AssignmentSubmissions
            .Include(s => s.Student)
            .Where(s => s.AssignmentId == assignmentId)
            .OrderByDescending(s => s.SubmittedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<AssignmentSubmission>> GetUngradedSubmissionsAsync(Guid assignmentId, CancellationToken ct = default)
        => await _context.AssignmentSubmissions
            .Include(s => s.Student)
            .Where(s => s.AssignmentId == assignmentId && s.GradedAt == null)
            .OrderBy(s => s.SubmittedAt)
            .ToListAsync(ct);
}
