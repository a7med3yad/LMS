using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using MediatR;

namespace LMS.Application.Command.Exam;

public class GradeOpenEndedHandler : IRequestHandler<GradeOpenEndedCommand>
{
    private readonly IUnitOfWork _uow;
    public GradeOpenEndedHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(GradeOpenEndedCommand req, CancellationToken ct)
    {
        var answer = await _uow.ExamAnswers.FindOneAsync(a => a.Id == req.Dto.AnswerId, ct)
            ?? throw new NotFoundException("Answer", req.Dto.AnswerId);

        var question = await _uow.Questions.GetByIdAsync(answer.QuestionId, ct)!;
        var exam = await _uow.Exams.GetByIdAsync(question!.ExamId, ct)!;

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, exam!.CourseId, ct))
            throw new ForbiddenException();

        answer.ManualGrade = req.Dto.Grade;
        _uow.ExamAnswers.Update(answer);
        await _uow.SaveChangesAsync(ct);
    }
}
