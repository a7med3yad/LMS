using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using MediatR;

namespace LMS.Application.Command.Material;

public class DeleteMaterialHandler : IRequestHandler<DeleteMaterialCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteMaterialHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(DeleteMaterialCommand req, CancellationToken ct)
    {
        var m = await _uow.Materials.GetByIdAsync(req.MaterialId, ct)
            ?? throw new NotFoundException("Material", req.MaterialId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, m.CourseId, ct))
            throw new ForbiddenException();

        _uow.Materials.Remove(m);
        await _uow.SaveChangesAsync(ct);
    }
}
