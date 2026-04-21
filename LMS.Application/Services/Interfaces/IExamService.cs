using LMS.Domain.DTOs.Exams;
using LMS.DTOs.Exams;

namespace LMS.Application.Services.Interfaces;

public interface IExamService
{
    Task<IEnumerable<ExamDto>> GetCourseExamsAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);

    Task<ExamDto> GetExamAsync(Guid examId, Guid requesterId, CancellationToken ct = default);

    Task<ExamDto> CreateExamAsync(Guid courseId, Guid instructorId, CreateExamDto dto, CancellationToken ct = default);

    Task<ExamDto> UpdateExamAsync(Guid examId, Guid instructorId, UpdateExamDto dto, CancellationToken ct = default);

    Task DeleteExamAsync(Guid examId, Guid instructorId, CancellationToken ct = default);

    Task PublishExamAsync(Guid examId, Guid instructorId, CancellationToken ct = default);

    // Questions
    Task<QuestionDto> AddQuestionAsync(Guid examId, Guid instructorId, CreateQuestionDto dto, CancellationToken ct = default);

    Task<QuestionDto> UpdateQuestionAsync(Guid questionId, Guid instructorId, UpdateQuestionDto dto, CancellationToken ct = default);

    Task DeleteQuestionAsync(Guid questionId, Guid instructorId, CancellationToken ct = default);

    // Student exam flow
    Task<StartExamResponseDto> StartExamAsync(Guid examId, Guid studentId, CancellationToken ct = default);

    Task<ExamAttemptDto> SubmitExamAsync(SubmitExamDto dto, Guid studentId, CancellationToken ct = default);

    Task<ExamAttemptDto> GetAttemptResultAsync(Guid attemptId, Guid requesterId, CancellationToken ct = default);

    Task<IEnumerable<ExamAttemptDto>> GetMyAttemptsAsync(Guid examId, Guid studentId, CancellationToken ct = default);

    // Instructor grading (open-ended)
    Task<IEnumerable<ExamAttemptDto>> GetAllAttemptsAsync(Guid examId, Guid instructorId, CancellationToken ct = default);

    Task GradeOpenEndedAnswerAsync(Guid examId, Guid instructorId, GradeOpenEndedDto dto, CancellationToken ct = default);
}
