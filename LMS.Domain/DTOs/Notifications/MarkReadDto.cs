namespace LMS.Domain.DTOs.Notifications;

public record MarkReadDto(
    IEnumerable<Guid> NotificationIds
);
