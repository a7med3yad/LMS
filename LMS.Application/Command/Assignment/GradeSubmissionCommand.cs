using LMS.Application.DTOs.Assignments;
using MediatR;

namespace LMS.Application.Command.Assignment;

public record GradeSubmissionCommand(
    Guid SubmissionId, Guid InstructorId, GradeSubmissionDto Dto)
    : IRequest<AssignmentSubmissionDto>;
