using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Exams;
using MediatR;

namespace LMS.Application.Command.Exam;

public class CreateExamHandler : IRequestHandler<CreateExamCommand, ExamDto>
{
    private readonly IUnitOfWork _uow;
    public CreateExamHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ExamDto> Handle(CreateExamCommand req, CancellationToken ct)
    {
        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, req.CourseId, ct))
            throw new ForbiddenException();

        var exam = new Domain.Models.Exam
        {
            Id = Guid.NewGuid(),
            TitleAr = req.Dto.TitleAr,
            TitleEn = req.Dto.TitleEn,
            DescriptionAr = req.Dto.DescriptionAr,
            DescriptionEn = req.Dto.DescriptionEn,
            DurationMinutes = req.Dto.DurationMinutes,
            PassScore = req.Dto.PassScore,
            MaxAttempts = req.Dto.MaxAttempts,
            AvailableFrom = req.Dto.AvailableFrom,
            AvailableUntil = req.Dto.AvailableUntil,
            CourseId = req.CourseId
        };

        await _uow.Exams.AddAsync(exam, ct);
        await _uow.SaveChangesAsync(ct);

        return new ExamDto(exam.Id, exam.TitleAr, exam.TitleEn,
            exam.DescriptionAr, exam.DescriptionEn, exam.DurationMinutes,
            exam.PassScore, exam.MaxAttempts, exam.IsPublished,
            exam.AvailableFrom, exam.AvailableUntil, exam.CourseId, 0, exam.CreatedAt);
    }
}