using MediatR;

namespace LMS.Application.Command.Material;

public record DeleteMaterialCommand(Guid MaterialId, Guid InstructorId) : IRequest;
