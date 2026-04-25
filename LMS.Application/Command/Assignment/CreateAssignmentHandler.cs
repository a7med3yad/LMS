using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Assignments;
using MediatR;

namespace LMS.Application.Command.Assignment;

public class CreateAssignmentHandler : IRequestHandler<CreateAssignmentCommand, AssignmentDto>
{
    private readonly IUnitOfWork _uow;
    public CreateAssignmentHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<AssignmentDto> Handle(CreateAssignmentCommand req, CancellationToken ct)
    {
        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, req.CourseId, ct))
            throw new ForbiddenException();

        var a = new Domain.Models.Assignment
        {
            Id = Guid.NewGuid(),
            TitleAr = req.Dto.TitleAr,
            TitleEn = req.Dto.TitleEn,
            DescriptionAr = req.Dto.DescriptionAr,
            DescriptionEn = req.Dto.DescriptionEn,
            SubmissionType = req.Dto.SubmissionType,
            DeadLine = req.Dto.DeadLine,
            MaxGrade = req.Dto.MaxGrade,
            CourseId = req.CourseId
        };

        await _uow.Assignments.AddAsync(a, ct);
        await _uow.SaveChangesAsync(ct);

        return new AssignmentDto(a.Id, a.TitleAr, a.TitleEn, a.DescriptionAr,
            a.DescriptionEn, a.SubmissionType, a.DeadLine, a.MaxGrade,
            a.IsPublished, a.CourseId, a.CreatedAt);
    }
}