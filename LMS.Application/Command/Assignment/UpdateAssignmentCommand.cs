using LMS.Application.DTOs.Assignments;
using MediatR;

namespace LMS.Application.Command.Assignment;

public record UpdateAssignmentCommand(Guid AssignmentId, Guid InstructorId, UpdateAssignmentDto Dto)
    : IRequest<AssignmentDto>;
