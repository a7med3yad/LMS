using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Materials;
using MediatR;

namespace LMS.Application.Command.Material;

public class UpdateMaterialHandler : IRequestHandler<UpdateMaterialCommand, MaterialDto>
{
    private readonly IUnitOfWork _uow;
    public UpdateMaterialHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<MaterialDto> Handle(UpdateMaterialCommand req, CancellationToken ct)
    {
        var material = await _uow.Materials.GetByIdAsync(req.MaterialId, ct)
            ?? throw new NotFoundException("Material", req.MaterialId);

        if (!await _uow.Courses.IsInstructorOfCourseAsync(req.InstructorId, material.CourseId, ct))
            throw new ForbiddenException();

        if (req.Dto.TitleAr is not null) material.TitleAr = req.Dto.TitleAr;
        if (req.Dto.TitleEn is not null) material.TitleEn = req.Dto.TitleEn;
        if (req.Dto.DescriptionAr is not null) material.DescriptionAr = req.Dto.DescriptionAr;
        if (req.Dto.DescriptionEn is not null) material.DescriptionEn = req.Dto.DescriptionEn;
        if (req.Dto.Type.HasValue) material.Type = req.Dto.Type.Value;
        if (req.Dto.ContentUrl is not null) material.ContentUrl = req.Dto.ContentUrl;
        if (req.Dto.TextContent is not null) material.TextContent = req.Dto.TextContent;
        if (req.Dto.Order.HasValue) material.Order = req.Dto.Order.Value;
        if (req.Dto.IsPublished.HasValue) material.IsPublished = req.Dto.IsPublished.Value;

        _uow.Materials.Update(material);
        await _uow.SaveChangesAsync(ct);

        return new MaterialDto(material.Id, material.TitleAr, material.TitleEn,
            material.DescriptionAr, material.DescriptionEn, material.Type,
            material.ContentUrl, material.TextContent, material.Order,
            material.IsPublished, material.CourseId, material.CreatedAt);
    }
}
