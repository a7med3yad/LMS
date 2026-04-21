using LMS.Application.Services.Interfaces;
using LMS.Domain.DTOs.Exams;
using LMS.DTOs.Exams;

namespace LMS.Application.Services;

public class ExamService : IExamService
{
    public Task<IEnumerable<ExamDto>> GetCourseExamsAsync(Guid courseId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ExamDto> GetExamAsync(Guid examId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ExamDto> CreateExamAsync(Guid courseId, Guid instructorId, CreateExamDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ExamDto> UpdateExamAsync(Guid examId, Guid instructorId, UpdateExamDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeleteExamAsync(Guid examId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task PublishExamAsync(Guid examId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<QuestionDto> AddQuestionAsync(Guid examId, Guid instructorId, CreateQuestionDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<QuestionDto> UpdateQuestionAsync(Guid questionId, Guid instructorId, UpdateQuestionDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeleteQuestionAsync(Guid questionId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<StartExamResponseDto> StartExamAsync(Guid examId, Guid studentId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ExamAttemptDto> SubmitExamAsync(SubmitExamDto dto, Guid studentId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<ExamAttemptDto> GetAttemptResultAsync(Guid attemptId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IEnumerable<ExamAttemptDto>> GetMyAttemptsAsync(Guid examId, Guid studentId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IEnumerable<ExamAttemptDto>> GetAllAttemptsAsync(Guid examId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task GradeOpenEndedAnswerAsync(Guid examId, Guid instructorId, GradeOpenEndedDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();
}
