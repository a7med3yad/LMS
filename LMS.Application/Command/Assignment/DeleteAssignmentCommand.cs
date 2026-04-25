using MediatR;

namespace LMS.Application.Command.Assignment;

public record DeleteAssignmentCommand(Guid AssignmentId, Guid InstructorId) : IRequest;
