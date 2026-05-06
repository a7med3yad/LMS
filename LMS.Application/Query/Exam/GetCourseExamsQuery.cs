using Dapper;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Exams;
using LMS.Domain.Services;
using MediatR;
using System.Data;

namespace LMS.Application.Query.Exam;

public record GetCourseExamsQuery(Guid CourseId, Guid RequesterId) : IRequest<IEnumerable<ExamDto>>;
public record GetExamQuery(Guid ExamId, Guid RequesterId) : IRequest<ExamDto>;
public record GetAttemptResultQuery(Guid AttemptId, Guid RequesterId) : IRequest<ExamAttemptDto>;
public record GetMyAttemptsQuery(Guid ExamId, Guid StudentId) : IRequest<IEnumerable<ExamAttemptDto>>;
public record GetAllAttemptsQuery(Guid ExamId, Guid InstructorId) : IRequest<IEnumerable<ExamAttemptDto>>;

// ─── GetCourseExams ───
public class GetCourseExamsHandler : IRequestHandler<GetCourseExamsQuery, IEnumerable<ExamDto>>
{
    private readonly ISqlQueryService _db;
    public GetCourseExamsHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<ExamDto>> Handle(GetCourseExamsQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT e.Id, e.TitleAr, e.TitleEn, e.DescriptionAr, e.DescriptionEn,
                   e.DurationMinutes, e.PassScore, e.MaxAttempts,
                   e.IsPublished, e.AvailableFrom, e.AvailableUntil,
                   e.CourseId,
                   (SELECT COUNT(*) FROM Questions q WHERE q.ExamId = e.Id) AS QuestionCount,
                   e.CreatedAt
            FROM Exams e INNER JOIN Courses c ON c.Id = e.CourseId
            WHERE e.CourseId = @CourseId
              AND (
                    c.InstructorId = @RequesterId
                    OR (e.IsPublished = 1
                        AND EXISTS (SELECT 1 FROM Enrollments en
                                    WHERE en.CourseId = e.CourseId
                                      AND en.StudentId = @RequesterId))
                  )
            ORDER BY e.CreatedAt DESC
            """;

        return await _db.QueryAsync<ExamDto>(sql, new { req.CourseId, req.RequesterId }, ct);
    }
}

// ─── GetExam ───
public class GetExamHandler : IRequestHandler<GetExamQuery, ExamDto>
{
    private readonly ISqlQueryService _db;
    public GetExamHandler(ISqlQueryService db) => _db = db;

    public async Task<ExamDto> Handle(GetExamQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT e.Id, e.TitleAr, e.TitleEn, e.DescriptionAr, e.DescriptionEn,
                   e.DurationMinutes, e.PassScore, e.MaxAttempts, e.IsPublished,
                   e.AvailableFrom, e.AvailableUntil, e.CourseId,
                   (SELECT COUNT(*) FROM Questions q WHERE q.ExamId = e.Id) AS QuestionCount,
                   e.CreatedAt
            FROM Exams e WHERE e.Id = @ExamId
            """;
        return await _db.QueryFirstOrDefaultAsync<ExamDto>(sql, new { req.ExamId }, ct)
            ?? throw new NotFoundException("Exam", req.ExamId);
    }
}

// ─── GetAttemptResult ───
public class GetAttemptResultHandler : IRequestHandler<GetAttemptResultQuery, ExamAttemptDto>
{
    private readonly ISqlQueryService _db;
    public GetAttemptResultHandler(ISqlQueryService db) => _db = db;

    public async Task<ExamAttemptDto> Handle(GetAttemptResultQuery req, CancellationToken ct)
    {
        const string headerSql = """
            SELECT a.Id, a.ExamId, e.TitleEn AS ExamTitle,
                   a.Score, a.IsPassed, a.StartedAt, a.SubmittedAt,
                   a.StudentId, e.CourseId
            FROM ExamAttempts a INNER JOIN Exams e ON e.Id = a.ExamId
            WHERE a.Id = @AttemptId
            """;

        var header = await _db.QueryFirstOrDefaultAsync<AttemptRow>(headerSql, new { req.AttemptId }, ct)
            ?? throw new NotFoundException("Attempt", req.AttemptId);

        // Check access: student or instructor
        if (header.StudentId != req.RequesterId)
        {
            var isInstructor = await _db.QueryFirstOrDefaultAsync<int>(
                "SELECT COUNT(1) FROM Courses WHERE Id=@CourseId AND InstructorId=@RequesterId",
                new { header.CourseId, req.RequesterId }, ct);
            if (isInstructor == 0) throw new ForbiddenException();
        }

        const string answersSql = """
            SELECT ans.QuestionId, q.TextEn AS QuestionText,
                   ans.SelectedChoiceId, ans.OpenAnswer,
                   CASE WHEN ch.IsCorrect = 1 THEN CAST(1 AS BIT)
                        WHEN ans.SelectedChoiceId IS NOT NULL THEN CAST(0 AS BIT)
                        ELSE NULL END AS IsCorrect,
                   ans.ManualGrade
            FROM ExamAnswers ans
            INNER JOIN Questions q ON q.Id = ans.QuestionId
            LEFT  JOIN QuestionChoices ch ON ch.Id = ans.SelectedChoiceId
            WHERE ans.AttemptId = @AttemptId
            """;

        var answers = await _db.QueryAsync<ExamAnswerResultDto>(answersSql, new { req.AttemptId }, ct);

        return new ExamAttemptDto(header.Id, header.ExamId, header.ExamTitle,
            header.Score, header.IsPassed, header.StartedAt, header.SubmittedAt, answers);
    }

    private record AttemptRow(Guid Id, Guid ExamId, string ExamTitle, int Score,
        bool IsPassed, DateTime StartedAt, DateTime? SubmittedAt, Guid StudentId, Guid CourseId);
}

// ─── GetMyAttempts ───
public class GetMyAttemptsHandler : IRequestHandler<GetMyAttemptsQuery, IEnumerable<ExamAttemptDto>>
{
    private readonly ISqlQueryService _db;
    public GetMyAttemptsHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<ExamAttemptDto>> Handle(GetMyAttemptsQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT a.Id, a.ExamId, e.TitleEn AS ExamTitle,
                   a.Score, a.IsPassed, a.StartedAt, a.SubmittedAt
            FROM ExamAttempts a INNER JOIN Exams e ON e.Id = a.ExamId
            WHERE a.ExamId = @ExamId AND a.StudentId = @StudentId
            ORDER BY a.StartedAt DESC
            """;
        var rows = await _db.QueryAsync<ARow>(sql, new { req.ExamId, req.StudentId }, ct);
        return rows.Select(r => new ExamAttemptDto(r.Id, r.ExamId, r.ExamTitle,
            r.Score, r.IsPassed, r.StartedAt, r.SubmittedAt, null));
    }

    private record ARow(Guid Id, Guid ExamId, string ExamTitle, int Score,
        bool IsPassed, DateTime StartedAt, DateTime? SubmittedAt);
}

// ─── GetAllAttempts (instructor) ───
public class GetAllAttemptsHandler : IRequestHandler<GetAllAttemptsQuery, IEnumerable<ExamAttemptDto>>
{
    private readonly ISqlQueryService _db;
    public GetAllAttemptsHandler(ISqlQueryService db) => _db = db;

    public async Task<IEnumerable<ExamAttemptDto>> Handle(GetAllAttemptsQuery req, CancellationToken ct)
    {
        const string sql = """
            SELECT a.Id, a.ExamId, e.TitleEn AS ExamTitle,
                   a.Score, a.IsPassed, a.StartedAt, a.SubmittedAt
            FROM ExamAttempts a
            INNER JOIN Exams e ON e.Id = a.ExamId
            INNER JOIN Courses c ON c.Id = e.CourseId
            WHERE a.ExamId = @ExamId AND c.InstructorId = @InstructorId
            ORDER BY a.StartedAt DESC
            """;
        var rows = await _db.QueryAsync<ARow>(sql, new { req.ExamId, req.InstructorId }, ct);
        return rows.Select(r => new ExamAttemptDto(r.Id, r.ExamId, r.ExamTitle,
            r.Score, r.IsPassed, r.StartedAt, r.SubmittedAt, null));
    }

    private record ARow(Guid Id, Guid ExamId, string ExamTitle, int Score,
        bool IsPassed, DateTime StartedAt, DateTime? SubmittedAt);
}
