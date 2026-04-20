using LMS.DTOs.Enrollments;

namespace LMS.Application.Services.Interfaces;

public interface IEnrollmentService
{
    Task<EnrollmentDto> EnrollAsync(Guid studentId, EnrollRequestDto dto, CancellationToken ct = default);

    Task<EnrollmentDto> GetEnrollmentAsync(Guid enrollmentId, Guid requesterId, CancellationToken ct = default);

    Task<IEnumerable<EnrollmentDto>> GetMyEnrollmentsAsync(Guid studentId, CancellationToken ct = default);

    Task<IEnumerable<EnrollmentDto>> GetCourseEnrollmentsAsync(Guid courseId, Guid instructorId, CancellationToken ct = default);

    Task CompleteEnrollmentAsync(Guid enrollmentId, Guid studentId, CancellationToken ct = default);

    Task SuspendEnrollmentAsync(Guid enrollmentId, Guid adminId, CancellationToken ct = default);

    Task<bool> IsEnrolledAsync(Guid studentId, Guid courseId, CancellationToken ct = default);
}
