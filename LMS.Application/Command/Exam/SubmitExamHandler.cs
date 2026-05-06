using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Exams;
using LMS.Domain.Models;
using LMS.Domain.Models.Enums;
using MediatR;

namespace LMS.Application.Command.Exam;

public class SubmitExamHandler : IRequestHandler<SubmitExamCommand, ExamAttemptDto>
{
    private readonly IUnitOfWork _uow;
    public SubmitExamHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ExamAttemptDto> Handle(SubmitExamCommand req, CancellationToken ct)
    {
        var attempt = await _uow.Exams.GetAttemptWithAnswersAsync(req.Dto.AttemptId, ct)
            ?? throw new NotFoundException("Attempt", req.Dto.AttemptId);

        if (attempt.StudentId != req.StudentId)
            throw new ForbiddenException();

        if (attempt.SubmittedAt is not null)
            throw new ConflictException("Attempt already submitted.");

        var exam = await _uow.Exams.GetExamWithQuestionsAsync(attempt.ExamId, ct)!;
        var questionsDict = exam!.Questions.ToDictionary(q => q.Id);

        int score = 0;
        var answers = new List<ExamAnswer>();
        var answerResults = new List<ExamAnswerResultDto>();

        foreach (var a in req.Dto.Answers)
        {
            if (!questionsDict.TryGetValue(a.QuestionId, out var question)) continue;

            var answer = new ExamAnswer
            {
                Id = Guid.NewGuid(),
                AttemptId = attempt.Id,
                QuestionId = a.QuestionId,
                SelectedChoiceId = a.SelectedChoiceId,
                OpenAnswer = a.OpenAnswer
            };

            bool? isCorrect = null;

            if (question.Type != QuestionType.OpenEnded && a.SelectedChoiceId.HasValue)
            {
                var choice = question.Choices.FirstOrDefault(c => c.Id == a.SelectedChoiceId.Value);
                isCorrect = choice?.IsCorrect ?? false;
                if (isCorrect == true) score += question.Points;
            }

            answers.Add(answer);
            answerResults.Add(new ExamAnswerResultDto(
                a.QuestionId, question.TextEn, a.SelectedChoiceId,
                a.OpenAnswer, isCorrect, null));
        }

        await _uow.ExamAnswers.AddRangeAsync(answers, ct);

        attempt.Score = score;
        attempt.IsPassed = score >= exam.PassScore;
        attempt.SubmittedAt = DateTime.UtcNow;
        _uow.ExamAttempts.Update(attempt);

        await _uow.SaveChangesAsync(ct);

        return new ExamAttemptDto(attempt.Id, attempt.ExamId, exam.TitleEn,
            attempt.Score, attempt.IsPassed, attempt.StartedAt,
            attempt.SubmittedAt, answerResults);
    }
}
