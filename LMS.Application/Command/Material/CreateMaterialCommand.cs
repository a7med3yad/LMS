using LMS.Application.DTOs.Materials;
using LMS.Domain.Models;
using MediatR;

namespace LMS.Application.Command.Material;

public record CreateMaterialCommand(Guid CourseId, Guid InstructorId, CreateMaterialDto Dto)
    : IRequest<MaterialDto>;
