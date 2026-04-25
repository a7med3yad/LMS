namespace LMS.Application.DTOs.Notifications;

public record MarkReadDto(
    IEnumerable<Guid> NotificationIds
);
