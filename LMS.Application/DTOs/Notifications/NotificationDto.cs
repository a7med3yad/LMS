
using LMS.Domain.Models.Enums;

namespace LMS.Application.DTOs.Notifications;

public record NotificationDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string BodyAr,
    string BodyEn,
    NotificationType Type,
    bool IsRead,
    string? ActionUrl,
    Guid? CourseId,
    Guid? ReferenceId,
    DateTime CreatedAt
);
