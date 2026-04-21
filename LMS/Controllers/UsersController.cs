using LMS.Application.Services.Interfaces;
using LMS.Domain.Common.Constants;
using LMS.Domain.Common.Pagination;
using LMS.Domain.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) => _userService = userService;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET api/v1/users/me
    [HttpGet("me")]
    [ProducesResponseType(typeof(UserProfileDto), 200)]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct)
    {
        var result = await _userService.GetProfileAsync(GetUserId(), ct);
        return Ok(result);
    }

    // PUT api/v1/users/me
    [HttpPut("me")]
    [ProducesResponseType(typeof(UserProfileDto), 200)]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileDto dto, CancellationToken ct)
    {
        var result = await _userService.UpdateProfileAsync(GetUserId(), dto, ct);
        return Ok(result);
    }

    // PATCH api/v1/users/me/avatar
    [HttpPatch("me/avatar")]
    [ProducesResponseType(typeof(UserProfileDto), 200)]
    public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarDto dto, CancellationToken ct)
    {
        var result = await _userService.UpdateAvatarAsync(GetUserId(), dto, ct);
        return Ok(result);
    }

    // GET api/v1/users  [Admin]
    [HttpGet]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(typeof(PagedResult<UserSummaryDto>), 200)]
    public async Task<IActionResult> GetAllUsers([FromQuery] PagedRequest request, CancellationToken ct)
    {
        var result = await _userService.GetAllUsersAsync(request, ct);
        return Ok(result);
    }

    // GET api/v1/users/{id}  [Admin]
    [HttpGet("{id:guid}")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(typeof(UserProfileDto), 200)]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken ct)
    {
        var result = await _userService.GetUserByIdAsync(id, ct);
        return Ok(result);
    }

    // PATCH api/v1/users/{id}/deactivate  [Admin]
    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeactivateUser(Guid id, CancellationToken ct)
    {
        await _userService.DeactivateUserAsync(id, ct);
        return NoContent();
    }

    // PATCH api/v1/users/{id}/activate  [Admin]
    [HttpPatch("{id:guid}/activate")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> ActivateUser(Guid id, CancellationToken ct)
    {
        await _userService.ActivateUserAsync(id, ct);
        return NoContent();
    }

    // GET api/v1/users/instructors  [Public]
    [HttpGet("instructors")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<UserSummaryDto>), 200)]
    public async Task<IActionResult> GetInstructors(CancellationToken ct)
    {
        var result = await _userService.GetInstructorsAsync(ct);
        return Ok(result);
    }
}
