using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Exams;
using MediatR;

namespace LMS.Application.Command.Exam;

public class UpdateExamHandler : IRequestHandler<UpdateExamCommand, ExamDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateExamHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ExamDto> Handle(UpdateExamCommand req, CancellationToken ct)
    {
        var exam = await _uow.Exams.GetExamWithQuestionsAsync(req.ExamId, ct)
            ?? throw new NotFoundException("Exam", req.ExamId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, exam.CourseId, ct))
            throw new ForbiddenException();

        if (req.Dto.TitleAr is not null) exam.TitleAr = req.Dto.TitleAr;
        if (req.Dto.TitleEn is not null) exam.TitleEn = req.Dto.TitleEn;
        if (req.Dto.DescriptionAr is not null) exam.DescriptionAr = req.Dto.DescriptionAr;
        if (req.Dto.DescriptionEn is not null) exam.DescriptionEn = req.Dto.DescriptionEn;
        if (req.Dto.DurationMinutes.HasValue) exam.DurationMinutes = req.Dto.DurationMinutes.Value;
        if (req.Dto.PassScore.HasValue) exam.PassScore = req.Dto.PassScore.Value;
        if (req.Dto.MaxAttempts.HasValue) exam.MaxAttempts = req.Dto.MaxAttempts.Value;
        if (req.Dto.IsPublished.HasValue) exam.IsPublished = req.Dto.IsPublished.Value;
        if (req.Dto.AvailableFrom.HasValue) exam.AvailableFrom = req.Dto.AvailableFrom.Value;
        if (req.Dto.AvailableUntil.HasValue) exam.AvailableUntil = req.Dto.AvailableUntil.Value;

        _uow.Exams.Update(exam);
        await _uow.SaveChangesAsync(ct);

        return new ExamDto(exam.Id, exam.TitleAr, exam.TitleEn,
            exam.DescriptionAr, exam.DescriptionEn, exam.DurationMinutes,
            exam.PassScore, exam.MaxAttempts, exam.IsPublished,
            exam.AvailableFrom, exam.AvailableUntil, exam.CourseId,
            exam.Questions.Count, exam.CreatedAt);
    }
}
