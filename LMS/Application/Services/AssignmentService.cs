using LMS.Application.Services.Interfaces;
using LMS.DTOs.Assignments;

namespace LMS.Application.Services;

public class AssignmentService : IAssignmentService
{
    public Task<IEnumerable<AssignmentDto>> GetCourseAssignmentsAsync(Guid courseId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<AssignmentDto> GetAssignmentAsync(Guid assignmentId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<AssignmentDto> CreateAssignmentAsync(Guid courseId, Guid instructorId, CreateAssignmentDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<AssignmentDto> UpdateAssignmentAsync(Guid assignmentId, Guid instructorId, UpdateAssignmentDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeleteAssignmentAsync(Guid assignmentId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task PublishAssignmentAsync(Guid assignmentId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<AssignmentSubmissionDto> SubmitAssignmentAsync(Guid assignmentId, Guid studentId, SubmitAssignmentDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<AssignmentSubmissionDto> GetMySubmissionAsync(Guid assignmentId, Guid studentId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IEnumerable<AssignmentSubmissionDto>> GetSubmissionsAsync(Guid assignmentId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<AssignmentSubmissionDto> GradeSubmissionAsync(Guid submissionId, Guid instructorId, GradeSubmissionDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();
}
