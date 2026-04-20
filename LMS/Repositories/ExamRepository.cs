using LMS.Data;
using LMS.Entities;
using LMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories;

public class ExamRepository : GenericRepository<Exam>, IExamRepository
{
    public ExamRepository(AppDbContext context) : base(context) { }

    public async Task<Exam?> GetExamWithQuestionsAsync(Guid examId, CancellationToken ct = default)
        => await _dbSet
            .Include(e => e.Questions)
                .ThenInclude(q => q.Choices)
            .FirstOrDefaultAsync(e => e.Id == examId, ct);

    public async Task<IEnumerable<Exam>> GetExamsByCourseAsync(Guid courseId, CancellationToken ct = default)
        => await _dbSet
            .Where(e => e.CourseId == courseId)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(ct);

    public async Task<ExamAttempt?> GetAttemptWithAnswersAsync(Guid attemptId, CancellationToken ct = default)
        => await _context.ExamAttempts
            .Include(a => a.Answers)
            .FirstOrDefaultAsync(a => a.Id == attemptId, ct);

    public async Task<IEnumerable<ExamAttempt>> GetAttemptsByStudentAsync(Guid studentId, Guid examId, CancellationToken ct = default)
        => await _context.ExamAttempts
            .Where(a => a.StudentId == studentId && a.ExamId == examId)
            .OrderByDescending(a => a.StartedAt)
            .ToListAsync(ct);

    public async Task<int> GetAttemptCountAsync(Guid studentId, Guid examId, CancellationToken ct = default)
        => await _context.ExamAttempts
            .CountAsync(a => a.StudentId == studentId && a.ExamId == examId, ct);

    public async Task<ExamAttempt?> GetActiveAttemptAsync(Guid studentId, Guid examId, CancellationToken ct = default)
        => await _context.ExamAttempts
            .FirstOrDefaultAsync(a => a.StudentId == studentId && a.ExamId == examId && a.SubmittedAt == null, ct);

    public async Task<IEnumerable<ExamAnswer>> GetOpenEndedUngradedAnswersAsync(Guid examId, CancellationToken ct = default)
        => await _context.ExamAnswers
            .Include(a => a.Question)
            .Where(a => a.Attempt!.ExamId == examId &&
                        a.Question.Type == Models.Enums.QuestionType.OpenEnded &&
                        a.ManualGrade == null)
            .ToListAsync(ct);
}
