using MediatR;

namespace LMS.Application.Command.Material;

public record ReorderMaterialsCommand(Guid CourseId, Guid InstructorId, IEnumerable<Guid> OrderedIds) : IRequest;
