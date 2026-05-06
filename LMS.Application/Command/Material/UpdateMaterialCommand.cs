using LMS.Application.DTOs.Materials;
using MediatR;

namespace LMS.Application.Command.Material;

public record UpdateMaterialCommand(Guid MaterialId, Guid InstructorId, UpdateMaterialDto Dto) : IRequest<MaterialDto>;
