using LMS.Application.DTOs.Assignments;
using MediatR;

namespace LMS.Application.Command.Assignment;

public record SubmitAssignmentCommand(
    Guid AssignmentId, Guid StudentId, SubmitAssignmentDto Dto)
    : IRequest<AssignmentSubmissionDto>;
