using LMS.Application.Services.Interfaces;
using LMS.Domain.Common.Constants;
using LMS.Domain.DTOs.Vouchers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/vouchers")]
[Authorize]
public class VouchersController : ControllerBase
{
    private readonly IVoucherService _voucherService;

    public VouchersController(IVoucherService voucherService) => _voucherService = voucherService;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET api/v1/courses/{courseId}/vouchers  [Instructor/Admin]
    [HttpGet]
    [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
    [ProducesResponseType(typeof(IEnumerable<VoucherDto>), 200)]
    public async Task<IActionResult> GetVouchers(Guid courseId, CancellationToken ct)
    {
        var result = await _voucherService.GetCourseVouchersAsync(courseId, GetUserId(), ct);
        return Ok(result);
    }

    // POST api/v1/courses/{courseId}/vouchers  [Instructor]
    [HttpPost]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(VoucherDto), 201)]
    public async Task<IActionResult> CreateVoucher(Guid courseId, [FromBody] CreateVoucherDto dto, CancellationToken ct)
    {
        var result = await _voucherService.CreateVoucherAsync(GetUserId(), dto, ct);
        return Created("", result);
    }

    // PUT api/v1/courses/{courseId}/vouchers/{id}  [Instructor]
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(VoucherDto), 200)]
    public async Task<IActionResult> UpdateVoucher(Guid courseId, Guid id, [FromBody] UpdateVoucherDto dto, CancellationToken ct)
    {
        var result = await _voucherService.UpdateVoucherAsync(id, GetUserId(), dto, ct);
        return Ok(result);
    }

    // DELETE api/v1/courses/{courseId}/vouchers/{id}  [Instructor]
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteVoucher(Guid courseId, Guid id, CancellationToken ct)
    {
        await _voucherService.DeleteVoucherAsync(id, GetUserId(), ct);
        return NoContent();
    }

    // POST api/v1/courses/{courseId}/vouchers/validate  [Student]
    [HttpPost("validate")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(VoucherDto), 200)]
    public async Task<IActionResult> ValidateVoucher(Guid courseId, [FromBody] string code, CancellationToken ct)
    {
        var result = await _voucherService.ValidateVoucherAsync(code, courseId, ct);
        return Ok(result);
    }
}
