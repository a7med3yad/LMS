using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Assignments;
using LMS.Domain.DTOs.Users;
using MediatR;

namespace LMS.Application.Command.Assignment;

public class GradeSubmissionHandler : IRequestHandler<GradeSubmissionCommand, AssignmentSubmissionDto>
{
    private readonly IUnitOfWork _uow;
    public GradeSubmissionHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<AssignmentSubmissionDto> Handle(GradeSubmissionCommand req, CancellationToken ct)
    {
        var submission = await _uow.AssignmentSubmissions
            .FindOneAsync(s => s.Id == req.SubmissionId, ct)
            ?? throw new NotFoundException("Submission", req.SubmissionId);

        var assignment = await _uow.Assignments.GetByIdAsync(submission.AssignmentId, ct)
            ?? throw new NotFoundException("Assignment", submission.AssignmentId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, assignment.CourseId, ct))
            throw new ForbiddenException();

        submission.Grade = req.Dto.Grade;
        submission.Feedback = req.Dto.Feedback;
        submission.GradedAt = DateTime.UtcNow;

        _uow.AssignmentSubmissions.Update(submission);
        await _uow.SaveChangesAsync(ct);

        var student = await _uow.Users.GetByIdAsync(submission.StudentId, ct);

        return new AssignmentSubmissionDto(submission.Id, submission.AssignmentId,
            assignment.TitleEn,
            new UserSummaryDto(student!.Id.ToString(), student.FullName,
                student.Email!, student.AvatarUrl, student.Role.ToString()),
            submission.TextAnswer, submission.FileUrl, submission.Grade,
            submission.Feedback, submission.SubmittedAt, submission.GradedAt);
    }
}
