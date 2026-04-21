using LMS.Application.Services.Interfaces;
using LMS.Domain.DTOs.Enrollments;

namespace LMS.Application.Services;

public class EnrollmentService : IEnrollmentService
{
    public Task<EnrollmentDto> EnrollAsync(Guid studentId, EnrollRequestDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<EnrollmentDto> GetEnrollmentAsync(Guid enrollmentId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IEnumerable<EnrollmentDto>> GetMyEnrollmentsAsync(Guid studentId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(Guid courseId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task CompleteEnrollmentAsync(Guid enrollmentId, Guid studentId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task SuspendEnrollmentAsync(Guid enrollmentId, Guid adminId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId, CancellationToken ct = default)
        => throw new NotImplementedException();
}
