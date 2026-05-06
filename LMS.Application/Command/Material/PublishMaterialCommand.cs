using MediatR;

namespace LMS.Application.Command.Material;

public record PublishMaterialCommand(Guid MaterialId, Guid InstructorId) : IRequest;
