using MediatR;

namespace LMS.Application.Command.Enrollment;

public record CompleteEnrollmentCommand(Guid EnrollmentId, Guid StudentId) : IRequest;
