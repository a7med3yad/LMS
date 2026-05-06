using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using MediatR;

namespace LMS.Application.Command.Material;

public class PublishMatexrialHandler : IRequestHandler<PublishMaterialCommand>
{
    private readonly IUnitOfWork _uow;
    public PublishMaterialHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(PublishMaterialCommand req, CancellationToken ct)
    {
        var m = await _uow.Materials.GetByIdAsync(req.MaterialId, ct)
            ?? throw new NotFoundException("Material", req.MaterialId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, m.CourseId, ct))
            throw new ForbiddenException();

        m.IsPublished = true;
        _uow.Materials.Update(m);
        await _uow.SaveChangesAsync(ct);
    }
}
