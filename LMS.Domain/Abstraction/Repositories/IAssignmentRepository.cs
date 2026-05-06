using LMS.Domain.Models;

namespace LMS.Domain.Abstraction.Repositories;

public interface IAssignmentRepository : IRepository<Assignment>
{
    Task<Assignment?> GetAssignmentWithSubmissionsAsync(Guid assignmentId, CancellationToken ct = default);

    Task<IEnumerable<Assignment>> GetAssignmentsByCourseAsync(Guid courseId, CancellationToken ct = default);

    Task<AssignmentSubmission?> GetSubmissionByStudentAsync(Guid assignmentId, Guid studentId, CancellationToken ct = default);

    Task<IEnumerable<AssignmentSubmission>> GetSubmissionsByAssignmentAsync(Guid assignmentId, CancellationToken ct = default);

    Task<IEnumerable<AssignmentSubmission>> GetUngradedSubmissionsAsync(Guid assignmentId, CancellationToken ct = default);
}
