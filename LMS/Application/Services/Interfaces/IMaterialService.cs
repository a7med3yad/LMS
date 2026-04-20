using LMS.DTOs.Materials;

namespace LMS.Application.Services.Interfaces;

public interface IMaterialService
{
    Task<IEnumerable<MaterialDto>> GetCourseMaterialsAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);

    Task<MaterialDto> GetMaterialAsync(Guid materialId, Guid requesterId, CancellationToken ct = default);

    Task<MaterialDto> CreateMaterialAsync(Guid courseId, Guid instructorId, CreateMaterialDto dto, CancellationToken ct = default);

    Task<MaterialDto> UpdateMaterialAsync(Guid materialId, Guid instructorId, UpdateMaterialDto dto, CancellationToken ct = default);

    Task DeleteMaterialAsync(Guid materialId, Guid instructorId, CancellationToken ct = default);

    Task PublishMaterialAsync(Guid materialId, Guid instructorId, CancellationToken ct = default);

    Task ReorderMaterialsAsync(Guid courseId, Guid instructorId, IEnumerable<Guid> orderedIds, CancellationToken ct = default);
}
