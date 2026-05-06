using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Assignments;
using LMS.Domain.DTOs.Users;
using MediatR;
using ValidationException = LMS.Application.Common.Exceptions.ValidationException;

namespace LMS.Application.Command.Assignment;

public class SubmitAssignmentHandler : IRequestHandler<SubmitAssignmentCommand, AssignmentSubmissionDto>
{
    private readonly IUnitOfWork _uow;
    public SubmitAssignmentHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<AssignmentSubmissionDto> Handle(SubmitAssignmentCommand req, CancellationToken ct)
    {
        var assignment = await _uow.Assignments.GetByIdAsync(req.AssignmentId, ct)
            ?? throw new NotFoundException("Assignment", req.AssignmentId);

        if (!assignment.IsPublished)
            throw new ForbiddenException("Assignment is not published.");

        if (DateTime.UtcNow > assignment.DeadLine)
            throw new ValidationException("Deadline has passed.");

        var existing = await _uow.Assignments.GetSubmissionByStudentAsync(req.AssignmentId, req.StudentId, ct);
        if (existing is not null)
            throw new ConflictException("You have already submitted this assignment.");

        var submission = new Domain.Models.AssignmentSubmission
        {
            Id = Guid.NewGuid(),
            TextAnswer = req.Dto.TextAnswer,
            FileUrl = req.Dto.FileUrl,
            AssignmentId = req.AssignmentId,
            StudentId = req.StudentId
        };

        await _uow.AssignmentSubmissions.AddAsync(submission, ct);
        await _uow.SaveChangesAsync(ct);

        var student = await _uow.Users.GetByIdAsync(req.StudentId, ct);

        return new AssignmentSubmissionDto(submission.Id, submission.AssignmentId,
            assignment.TitleEn,
            new UserSummaryDto(student!.Id.ToString(), student.FullName,
                student.Email!, student.AvatarUrl, student.Role.ToString()),
            submission.TextAnswer, submission.FileUrl, submission.Grade,
            submission.Feedback, submission.SubmittedAt, submission.GradedAt);
    }
}
