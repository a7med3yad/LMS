using LMS.Domain.Models;
using LMS.Domain.Repositories.Intercaces;
using LMS.Domain.Repositories.Interfaces;

namespace LMS.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // Repositories
    ICourseRepository Courses { get; }
    IEnrollmentRepository Enrollments { get; }
    IUserRepository Users { get; }
    IAssignmentRepository Assignments { get; }
    IExamRepository Exams { get; }

    // Generic access for entities without dedicated repositories
    IRepository<Material> Materials { get; }
    IRepository<Notification> Notifications { get; }
    IRepository<Voucher> Vouchers { get; }
    IRepository<ExamAttempt> ExamAttempts { get; }
    IRepository<ExamAnswer> ExamAnswers { get; }
    IRepository<Question> Questions { get; }
    IRepository<QuestionChoice> QuestionChoices { get; }
    IRepository<AssignmentSubmission> AssignmentSubmissions { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    Task BeginTransactionAsync(CancellationToken ct = default);
    Task CommitTransactionAsync(CancellationToken ct = default);
    Task RollbackTransactionAsync(CancellationToken ct = default);
}
