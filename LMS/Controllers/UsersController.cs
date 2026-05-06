using LMS.Application.Command.User;
using LMS.Application.Common.Pagination;
using LMS.Application.DTOs.Users;
using LMS.Application.Query.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/users")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;
    public UsersController(ISender sender) => _sender = sender;

    private Guid UserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct)
        => Ok(await _sender.Send(new GetMyProfileQuery(UserId()), ct));

    [HttpPut("me")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new UpdateProfileCommand(UserId(), dto), ct));

    [HttpPatch("me/avatar")]
    public async Task<IActionResult> UpdateAvatar(UpdateAvatarDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new UpdateAvatarCommand(UserId(), dto), ct));

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers([FromQuery] PagedRequest request, CancellationToken ct)
        => Ok(await _sender.Send(new GetAllUsersQuery(request), ct));

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetUserByIdQuery(id), ct));

    [HttpPatch("{id:guid}/deactivate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeactivateUser(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeactivateUserCommand(id), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/activate")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ActivateUser(Guid id, CancellationToken ct)
    {
        await _sender.Send(new ActivateUserCommand(id), ct);
        return NoContent();
    }

    [HttpGet("instructors")]
    [AllowAnonymous]
    public async Task<IActionResult> GetInstructors(CancellationToken ct)
        => Ok(await _sender.Send(new GetInstructorsQuery(), ct));
}
