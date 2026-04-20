using LMS.DTOs.Assignments;

namespace LMS.Application.Services.Interfaces;

public interface IAssignmentService
{
    Task<IEnumerable<AssignmentDto>> GetCourseAssignmentsAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);

    Task<AssignmentDto> GetAssignmentAsync(Guid assignmentId, Guid requesterId, CancellationToken ct = default);

    Task<AssignmentDto> CreateAssignmentAsync(Guid courseId, Guid instructorId, CreateAssignmentDto dto, CancellationToken ct = default);

    Task<AssignmentDto> UpdateAssignmentAsync(Guid assignmentId, Guid instructorId, UpdateAssignmentDto dto, CancellationToken ct = default);

    Task DeleteAssignmentAsync(Guid assignmentId, Guid instructorId, CancellationToken ct = default);

    Task PublishAssignmentAsync(Guid assignmentId, Guid instructorId, CancellationToken ct = default);

    // Student actions
    Task<AssignmentSubmissionDto> SubmitAssignmentAsync(Guid assignmentId, Guid studentId, SubmitAssignmentDto dto, CancellationToken ct = default);

    Task<AssignmentSubmissionDto> GetMySubmissionAsync(Guid assignmentId, Guid studentId, CancellationToken ct = default);

    // Instructor actions
    Task<IEnumerable<AssignmentSubmissionDto>> GetSubmissionsAsync(Guid assignmentId, Guid instructorId, CancellationToken ct = default);

    Task<AssignmentSubmissionDto> GradeSubmissionAsync(Guid submissionId, Guid instructorId, GradeSubmissionDto dto, CancellationToken ct = default);
}
