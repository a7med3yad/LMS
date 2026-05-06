using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using MediatR;

namespace LMS.Application.Command.Exam;

public class PublishExamHandler : IRequestHandler<PublishExamCommand>
{
    private readonly IUnitOfWork _uow;
    public PublishExamHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(PublishExamCommand req, CancellationToken ct)
    {
        var exam = await _uow.Exams.GetByIdAsync(req.ExamId, ct)
            ?? throw new NotFoundException("Exam", req.ExamId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, exam.CourseId, ct))
            throw new ForbiddenException();

        exam.IsPublished = true;
        _uow.Exams.Update(exam);
        await _uow.SaveChangesAsync(ct);
    }
}
