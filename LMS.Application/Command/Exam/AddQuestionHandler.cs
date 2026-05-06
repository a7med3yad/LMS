using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Exams;
using LMS.Domain.Models;
using MediatR;

namespace LMS.Application.Command.Exam;

public class AddQuestionHandler : IRequestHandler<AddQuestionCommand, QuestionDto>
{
    private readonly IUnitOfWork _uow;
    public AddQuestionHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<QuestionDto> Handle(AddQuestionCommand req, CancellationToken ct)
    {
        var exam = await _uow.Exams.GetByIdAsync(req.ExamId, ct)
            ?? throw new NotFoundException("Exam", req.ExamId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, exam.CourseId, ct))
            throw new ForbiddenException();

        var question = new Question
        {
            Id = Guid.NewGuid(),
            TextAr = req.Dto.TextAr,
            TextEn = req.Dto.TextEn,
            Type = req.Dto.Type,
            Points = req.Dto.Points,
            Order = req.Dto.Order,
            ExamId = req.ExamId
        };

        await _uow.Questions.AddAsync(question, ct);

        var choiceDtos = new List<QuestionChoiceDto>();
        if (req.Dto.Choices is not null)
        {
            foreach (var c in req.Dto.Choices)
            {
                var choice = new QuestionChoice
                {
                    Id = Guid.NewGuid(),
                    TextAr = c.TextAr,
                    TextEn = c.TextEn,
                    IsCorrect = c.IsCorrect,
                    QuestionId = question.Id
                };
                await _uow.QuestionChoices.AddAsync(choice, ct);
                choiceDtos.Add(new QuestionChoiceDto(choice.Id, choice.TextAr, choice.TextEn, choice.IsCorrect));
            }
        }

        await _uow.SaveChangesAsync(ct);

        return new QuestionDto(question.Id, question.TextAr, question.TextEn,
            question.Type, question.Points, question.Order, choiceDtos);
    }
}
