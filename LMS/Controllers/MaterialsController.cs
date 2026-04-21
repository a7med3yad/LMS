using LMS.Application.Services.Interfaces;
using LMS.Domain.Common.Constants;
using LMS.Domain.DTOs.Materials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/materials")]
[Authorize]
public class MaterialsController : ControllerBase
{
    private readonly IMaterialService _materialService;

    public MaterialsController(IMaterialService materialService) => _materialService = materialService;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET api/v1/courses/{courseId}/materials
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MaterialDto>), 200)]
    public async Task<IActionResult> GetMaterials(Guid courseId, CancellationToken ct)
    {
        var result = await _materialService.GetCourseMaterialsAsync(courseId, GetUserId(), ct);
        return Ok(result);
    }

    // GET api/v1/courses/{courseId}/materials/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MaterialDto), 200)]
    public async Task<IActionResult> GetMaterial(Guid courseId, Guid id, CancellationToken ct)
    {
        var result = await _materialService.GetMaterialAsync(id, GetUserId(), ct);
        return Ok(result);
    }

    // POST api/v1/courses/{courseId}/materials  [Instructor]
    [HttpPost]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(MaterialDto), 201)]
    public async Task<IActionResult> CreateMaterial(Guid courseId, [FromBody] CreateMaterialDto dto, CancellationToken ct)
    {
        var result = await _materialService.CreateMaterialAsync(courseId, GetUserId(), dto, ct);
        return CreatedAtAction(nameof(GetMaterial), new { courseId, id = result.Id }, result);
    }

    // PUT api/v1/courses/{courseId}/materials/{id}  [Instructor]
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(MaterialDto), 200)]
    public async Task<IActionResult> UpdateMaterial(Guid courseId, Guid id, [FromBody] UpdateMaterialDto dto, CancellationToken ct)
    {
        var result = await _materialService.UpdateMaterialAsync(id, GetUserId(), dto, ct);
        return Ok(result);
    }

    // PATCH api/v1/courses/{courseId}/materials/{id}/publish  [Instructor]
    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> PublishMaterial(Guid courseId, Guid id, CancellationToken ct)
    {
        await _materialService.PublishMaterialAsync(id, GetUserId(), ct);
        return NoContent();
    }

    // PATCH api/v1/courses/{courseId}/materials/reorder  [Instructor]
    [HttpPatch("reorder")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> ReorderMaterials(Guid courseId, [FromBody] IEnumerable<Guid> orderedIds, CancellationToken ct)
    {
        await _materialService.ReorderMaterialsAsync(courseId, GetUserId(), orderedIds, ct);
        return NoContent();
    }

    // DELETE api/v1/courses/{courseId}/materials/{id}  [Instructor]
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteMaterial(Guid courseId, Guid id, CancellationToken ct)
    {
        await _materialService.DeleteMaterialAsync(id, GetUserId(), ct);
        return NoContent();
    }
}
