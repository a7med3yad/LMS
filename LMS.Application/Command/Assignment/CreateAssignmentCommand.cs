using LMS.Application.DTOs.Assignments;
using MediatR;

namespace LMS.Application.Command.Assignment;

public record CreateAssignmentCommand(Guid CourseId, Guid InstructorId, CreateAssignmentDto Dto)
    : IRequest<AssignmentDto>;
