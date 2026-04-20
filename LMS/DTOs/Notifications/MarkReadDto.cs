namespace LMS.DTOs.Notifications;

public record MarkReadDto(
    IEnumerable<Guid> NotificationIds
);
