using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Assignments;
using MediatR;

namespace LMS.Application.Command.Assignment;

public class UpdateAssignmentHandler : IRequestHandler<UpdateAssignmentCommand, AssignmentDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateAssignmentHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<AssignmentDto> Handle(UpdateAssignmentCommand req, CancellationToken ct)
    {
        var a = await _uow.Assignments.GetByIdAsync(req.AssignmentId, ct)
            ?? throw new NotFoundException("Assignment", req.AssignmentId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, a.CourseId, ct))
            throw new ForbiddenException();

        if (req.Dto.TitleAr is not null) a.TitleAr = req.Dto.TitleAr;
        if (req.Dto.TitleEn is not null) a.TitleEn = req.Dto.TitleEn;
        if (req.Dto.DescriptionAr is not null) a.DescriptionAr = req.Dto.DescriptionAr;
        if (req.Dto.DescriptionEn is not null) a.DescriptionEn = req.Dto.DescriptionEn;
        if (req.Dto.SubmissionType.HasValue) a.SubmissionType = req.Dto.SubmissionType.Value;
        if (req.Dto.DeadLine.HasValue) a.DeadLine = req.Dto.DeadLine.Value;
        if (req.Dto.MaxGrade.HasValue) a.MaxGrade = req.Dto.MaxGrade.Value;
        if (req.Dto.IsPublished.HasValue) a.IsPublished = req.Dto.IsPublished.Value;

        _uow.Assignments.Update(a);
        await _uow.SaveChangesAsync(ct);

        return new AssignmentDto(a.Id, a.TitleAr, a.TitleEn, a.DescriptionAr,
            a.DescriptionEn, a.SubmissionType, a.DeadLine, a.MaxGrade,
            a.IsPublished, a.CourseId, a.CreatedAt);
    }
}
