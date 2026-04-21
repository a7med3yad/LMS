
using LMS.Domain.Models;

namespace LMS.Domain.Repositories.Interfaces;

public interface IExamRepository : IRepository<Exam>
{
    Task<Exam?> GetExamWithQuestionsAsync(Guid examId, CancellationToken ct = default);

    Task<IEnumerable<Exam>> GetExamsByCourseAsync(Guid courseId, CancellationToken ct = default);

    Task<ExamAttempt?> GetAttemptWithAnswersAsync(Guid attemptId, CancellationToken ct = default);

    Task<IEnumerable<ExamAttempt>> GetAttemptsByStudentAsync(Guid studentId, Guid examId, CancellationToken ct = default);

    Task<int> GetAttemptCountAsync(Guid studentId, Guid examId, CancellationToken ct = default);

    Task<ExamAttempt?> GetActiveAttemptAsync(Guid studentId, Guid examId, CancellationToken ct = default);

    Task<IEnumerable<ExamAnswer>> GetOpenEndedUngradedAnswersAsync(Guid examId, CancellationToken ct = default);
}
