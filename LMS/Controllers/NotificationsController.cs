using LMS.Application.Services.Interfaces;
using LMS.Common.Pagination;
using LMS.DTOs.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;

    public NotificationsController(INotificationService notificationService) => _notificationService = notificationService;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET api/v1/notifications
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<NotificationDto>), 200)]
    public async Task<IActionResult> GetMyNotifications([FromQuery] PagedRequest request, CancellationToken ct)
    {
        var result = await _notificationService.GetMyNotificationsAsync(GetUserId(), request, ct);
        return Ok(result);
    }

    // GET api/v1/notifications/unread-count
    [HttpGet("unread-count")]
    [ProducesResponseType(typeof(int), 200)]
    public async Task<IActionResult> GetUnreadCount(CancellationToken ct)
    {
        var count = await _notificationService.GetUnreadCountAsync(GetUserId(), ct);
        return Ok(new { UnreadCount = count });
    }

    // PATCH api/v1/notifications/mark-read
    [HttpPatch("mark-read")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> MarkAsRead([FromBody] MarkReadDto dto, CancellationToken ct)
    {
        await _notificationService.MarkAsReadAsync(GetUserId(), dto, ct);
        return NoContent();
    }

    // PATCH api/v1/notifications/mark-all-read
    [HttpPatch("mark-all-read")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> MarkAllAsRead(CancellationToken ct)
    {
        await _notificationService.MarkAllAsReadAsync(GetUserId(), ct);
        return NoContent();
    }

    // DELETE api/v1/notifications/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteNotification(Guid id, CancellationToken ct)
    {
        await _notificationService.DeleteNotificationAsync(id, GetUserId(), ct);
        return NoContent();
    }
}
