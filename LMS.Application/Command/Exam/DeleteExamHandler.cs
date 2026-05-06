using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using MediatR;

namespace LMS.Application.Command.Exam;

public class DeleteExamHandler : IRequestHandler<DeleteExamCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteExamHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteExamCommand req, CancellationToken ct)
    {
        var exam = await _uow.Exams.GetByIdAsync(req.ExamId, ct)
            ?? throw new NotFoundException("Exam", req.ExamId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, exam.CourseId, ct))
            throw new ForbiddenException();

        _uow.Exams.Remove(exam);
        await _uow.SaveChangesAsync(ct);
    }
}
