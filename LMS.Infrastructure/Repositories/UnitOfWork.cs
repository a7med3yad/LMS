using LMS.Domain.Models;
using LMS.Domain.Repositories.Intercaces;
using LMS.Domain.Repositories.Interfaces;
using LMS.Infrastructure.Persistence;
using LMS.Infrastructure.Repositories;
using LMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace LMS.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    private ICourseRepository? _courses;
    private IEnrollmentRepository? _enrollments;
    private IUserRepository? _users;
    private IAssignmentRepository? _assignments;
    private IExamRepository? _exams;

    private IRepository<Material>? _materials;
    private IRepository<Notification>? _notifications;
    private IRepository<Voucher>? _vouchers;
    private IRepository<ExamAttempt>? _examAttempts;
    private IRepository<ExamAnswer>? _examAnswers;
    private IRepository<Question>? _questions;
    private IRepository<QuestionChoice>? _questionChoices;
    private IRepository<AssignmentSubmission>? _assignmentSubmissions;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public ICourseRepository Courses => _courses ??= new CourseRepository(_context);
    public IEnrollmentRepository Enrollments => _enrollments ??= new EnrollmentRepository(_context);
    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IAssignmentRepository Assignments => _assignments ??= new AssignmentRepository(_context);
    public IExamRepository Exams => _exams ??= new ExamRepository(_context);

    public IRepository<Material> Materials => _materials ??= new GenericRepository<Material>(_context);
    public IRepository<Notification> Notifications => _notifications ??= new GenericRepository<Notification>(_context);
    public IRepository<Voucher> Vouchers => _vouchers ??= new GenericRepository<Voucher>(_context);
    public IRepository<ExamAttempt> ExamAttempts => _examAttempts ??= new GenericRepository<ExamAttempt>(_context);
    public IRepository<ExamAnswer> ExamAnswers => _examAnswers ??= new GenericRepository<ExamAnswer>(_context);
    public IRepository<Question> Questions => _questions ??= new GenericRepository<Question>(_context);
    public IRepository<QuestionChoice> QuestionChoices => _questionChoices ??= new GenericRepository<QuestionChoice>(_context);
    public IRepository<AssignmentSubmission> AssignmentSubmissions => _assignmentSubmissions ??= new GenericRepository<AssignmentSubmission>(_context);

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public async Task BeginTransactionAsync(CancellationToken ct = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(ct);
    }

    public async Task CommitTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken ct = default)
    {
        if (_transaction is not null)
        {
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
