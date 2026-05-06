using MediatR;

namespace LMS.Application.Command.Assignment;

public record PublishAssignmentCommand(Guid AssignmentId, Guid InstructorId) : IRequest;
