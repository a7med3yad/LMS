using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Exams;
using LMS.Domain.Models;
using MediatR;

namespace LMS.Application.Command.Exam;

public class StartExamHandler : IRequestHandler<StartExamCommand, StartExamResponseDto>
{
    private readonly IUnitOfWork _uow;
    public StartExamHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<StartExamResponseDto> Handle(StartExamCommand req, CancellationToken ct)
    {
        var exam = await _uow.Exams.GetExamWithQuestionsAsync(req.ExamId, ct)
            ?? throw new NotFoundException("Exam", req.ExamId);

        if (!exam.IsPublished)
            throw new ForbiddenException("Exam is not published.");

        var attemptCount = await _uow.Exams.GetAttemptCountAsync(req.StudentId, req.ExamId, ct);
        if (attemptCount >= exam.MaxAttempts)
            throw new ConflictException("Maximum attempts reached.");

        var active = await _uow.Exams.GetActiveAttemptAsync(req.StudentId, req.ExamId, ct);
        if (active is not null)
            throw new ConflictException("You already have an active attempt.");

        var attempt = new ExamAttempt
        {
            Id = Guid.NewGuid(),
            ExamId = req.ExamId,
            StudentId = req.StudentId,
            StartedAt = DateTime.UtcNow
        };

        await _uow.ExamAttempts.AddAsync(attempt, ct);
        await _uow.SaveChangesAsync(ct);

        var questions = exam.Questions.Select(q => new QuestionDto(
            q.Id, q.TextAr, q.TextEn, q.Type, q.Points, q.Order,
            q.Choices.Select(c => new QuestionChoiceDto(c.Id, c.TextAr, c.TextEn, null))
        ));

        return new StartExamResponseDto(attempt.Id, exam.Id, exam.TitleAr, exam.TitleEn,
            exam.DurationMinutes, attempt.StartedAt,
            attempt.StartedAt.AddMinutes(exam.DurationMinutes), questions);
    }
}
