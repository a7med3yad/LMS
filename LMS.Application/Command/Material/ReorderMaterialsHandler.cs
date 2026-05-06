using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using MediatR;

namespace LMS.Application.Command.Material;

public class ReorderMaterialsHandler : IRequestHandler<ReorderMaterialsCommand>
{
    private readonly IUnitOfWork _uow;
    public ReorderMaterialsHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(ReorderMaterialsCommand req, CancellationToken ct)
    {
        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, req.CourseId, ct))
            throw new ForbiddenException();

        var list = req.OrderedIds.ToList();
        for (int i = 0; i < list.Count; i++)
        {
            var m = await _uow.Materials.GetByIdAsync(list[i], ct);
            if (m is null || m.CourseId != req.CourseId) continue;
            m.Order = i + 1;
            _uow.Materials.Update(m);
        }
        await _uow.SaveChangesAsync(ct);
    }
}