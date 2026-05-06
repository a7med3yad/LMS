using LMS.Application.Command.Notification;
using LMS.Application.Common.Pagination;
using LMS.Application.DTOs.Notifications;
using LMS.Application.Query.Notification;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly ISender _sender;
    public NotificationsController(ISender sender) => _sender = sender;

    private Guid UserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] PagedRequest request, CancellationToken ct)
        => Ok(await _sender.Send(new GetMyNotificationsQuery(UserId(), request), ct));

    [HttpGet("unread-count")]
    public async Task<IActionResult> GetUnreadCount(CancellationToken ct)
        => Ok(await _sender.Send(new GetUnreadCountQuery(UserId()), ct));

    [HttpPatch("mark-read")]
    public async Task<IActionResult> MarkRead(MarkReadDto dto, CancellationToken ct)
    {
        await _sender.Send(new MarkAsReadCommand(UserId(), dto), ct);
        return NoContent();
    }

    [HttpPatch("mark-all-read")]
    public async Task<IActionResult> MarkAllRead(CancellationToken ct)
    {
        await _sender.Send(new MarkAllAsReadCommand(UserId()), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeleteNotificationCommand(id, UserId()), ct);
        return NoContent();
    }
}
