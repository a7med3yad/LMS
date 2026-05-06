using LMS.Application.Command.Voucher;
using LMS.Application.DTOs.Vouchers;
using LMS.Application.Query.Voucher;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/vouchers")]
[Authorize]
public class VouchersController : ControllerBase
{
    private readonly ISender _sender;
    public VouchersController(ISender sender) => _sender = sender;

    private Guid UserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> GetVouchers(Guid courseId, CancellationToken ct)
        => Ok(await _sender.Send(new GetCourseVouchersQuery(courseId, UserId()), ct));

    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Create(Guid courseId, CreateVoucherDto dto, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateVoucherCommand(UserId(), dto), ct);
        return CreatedAtAction(nameof(GetVouchers), new { courseId }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Update(Guid id, UpdateVoucherDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new UpdateVoucherCommand(id, UserId(), dto), ct));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeleteVoucherCommand(id, UserId()), ct);
        return NoContent();
    }

    [HttpPost("validate")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Validate(Guid courseId, [FromQuery] string code, CancellationToken ct)
        => Ok(await _sender.Send(new ValidateVoucherQuery(code, courseId), ct));
}
