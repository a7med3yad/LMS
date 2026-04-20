using LMS.Application.Services.Interfaces;
using LMS.DTOs.Materials;

namespace LMS.Application.Services;

public class MaterialService : IMaterialService
{
    public Task<IEnumerable<MaterialDto>> GetCourseMaterialsAsync(Guid courseId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<MaterialDto> GetMaterialAsync(Guid materialId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<MaterialDto> CreateMaterialAsync(Guid courseId, Guid instructorId, CreateMaterialDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<MaterialDto> UpdateMaterialAsync(Guid materialId, Guid instructorId, UpdateMaterialDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeleteMaterialAsync(Guid materialId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task PublishMaterialAsync(Guid materialId, Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task ReorderMaterialsAsync(Guid courseId, Guid instructorId, IEnumerable<Guid> orderedIds, CancellationToken ct = default)
        => throw new NotImplementedException();
}
