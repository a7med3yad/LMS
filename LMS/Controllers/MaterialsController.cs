using LMS.Application.Command.Material;
using LMS.Application.DTOs.Materials;
using LMS.Application.Query.Material;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/materials")]
[Authorize]
public class MaterialsController : ControllerBase
{
    private readonly ISender _sender;
    public MaterialsController(ISender sender) => _sender = sender;

    private Guid UserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetMaterials(Guid courseId, CancellationToken ct)
        => Ok(await _sender.Send(new GetCourseMaterialsQuery(courseId, UserId()), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetMaterial(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetMaterialQuery(id, UserId()), ct));

    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Create(Guid courseId, CreateMaterialDto dto, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateMaterialCommand(courseId, UserId(), dto), ct);
        return CreatedAtAction(nameof(GetMaterial), new { courseId, id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Update(Guid id, UpdateMaterialDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new UpdateMaterialCommand(id, UserId(), dto), ct));

    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
    {
        await _sender.Send(new PublishMaterialCommand(id, UserId()), ct);
        return NoContent();
    }

    [HttpPatch("reorder")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Reorder(Guid courseId, [FromBody] IEnumerable<Guid> orderedIds, CancellationToken ct)
    {
        await _sender.Send(new ReorderMaterialsCommand(courseId, UserId(), orderedIds), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeleteMaterialCommand(id, UserId()), ct);
        return NoContent();
    }
}
