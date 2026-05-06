using MediatR;

namespace LMS.Application.Command.Enrollment;

public record SuspendEnrollmentCommand(Guid EnrollmentId, Guid AdminId) : IRequest;
