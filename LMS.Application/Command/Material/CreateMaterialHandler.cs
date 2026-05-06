using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Materials;
using MediatR;

namespace LMS.Application.Command.Material;

public class CreateMaterialHandler : IRequestHandler<CreateMaterialCommand, MaterialDto>
{
    private readonly IUnitOfWork _uow;
    public CreateMaterialHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<MaterialDto> Handle(CreateMaterialCommand request, CancellationToken ct)
    {
        if (!await _uow.Courses.IsInstructorOfCourseAsync(request.InstructorId, request.CourseId, ct))
            throw new ForbiddenException("You do not own this course.");

        var material = new Domain.Models.Material
        {
            Id = Guid.NewGuid(),
            TitleAr = request.Dto.TitleAr,
            TitleEn = request.Dto.TitleEn,
            DescriptionAr = request.Dto.DescriptionAr,
            DescriptionEn = request.Dto.DescriptionEn,
            Type = request.Dto.Type,
            ContentUrl = request.Dto.ContentUrl,
            TextContent = request.Dto.TextContent,
            Order = request.Dto.Order,
            CourseId = request.CourseId
        };

        await _uow.Materials.AddAsync(material, ct);
        await _uow.SaveChangesAsync(ct);

        return new MaterialDto(material.Id, material.TitleAr, material.TitleEn,
            material.DescriptionAr, material.DescriptionEn, material.Type,
            material.ContentUrl, material.TextContent, material.Order,
            material.IsPublished, material.CourseId, material.CreatedAt);
    }
}