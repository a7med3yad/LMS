using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Exams;
using MediatR;

namespace LMS.Application.Command.Exam;

public class UpdateQuestionHandler : IRequestHandler<UpdateQuestionCommand, QuestionDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateQuestionHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<QuestionDto> Handle(UpdateQuestionCommand req, CancellationToken ct)
    {
        var question = await _uow.Questions.GetByIdAsync(req.QuestionId, ct)
            ?? throw new NotFoundException("Question", req.QuestionId);

        var exam = await _uow.Exams.GetByIdAsync(question.ExamId, ct)!;
        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, exam!.CourseId, ct))
            throw new ForbiddenException();

        if (req.Dto.TextAr is not null) question.TextAr = req.Dto.TextAr;
        if (req.Dto.TextEn is not null) question.TextEn = req.Dto.TextEn;
        if (req.Dto.Type.HasValue) question.Type = req.Dto.Type.Value;
        if (req.Dto.Points.HasValue) question.Points = req.Dto.Points.Value;
        if (req.Dto.Order.HasValue) question.Order = req.Dto.Order.Value;

        _uow.Questions.Update(question);
        await _uow.SaveChangesAsync(ct);

        var choices = await _uow.QuestionChoices.FindAsync(c => c.QuestionId == question.Id, ct);

        return new QuestionDto(question.Id, question.TextAr, question.TextEn,
            question.Type, question.Points, question.Order,
            choices.Select(c => new QuestionChoiceDto(c.Id, c.TextAr, c.TextEn, c.IsCorrect)));
    }
}

public class DeleteQuestionHandler : IRequestHandler<DeleteQuestionCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteQuestionHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteQuestionCommand req, CancellationToken ct)
    {
        var question = await _uow.Questions.GetByIdAsync(req.QuestionId, ct)
            ?? throw new NotFoundException("Question", req.QuestionId);

        var exam = await _uow.Exams.GetByIdAsync(question.ExamId, ct)!;
        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, exam!.CourseId, ct))
            throw new ForbiddenException();

        _uow.Questions.Remove(question);
        await _uow.SaveChangesAsync(ct);
    }
}
