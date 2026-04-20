using LMS.DTOs.Notifications;
using LMS.Common.Pagination;

namespace LMS.Application.Services.Interfaces;

public interface INotificationService
{
    Task<PagedResult<NotificationDto>> GetMyNotificationsAsync(Guid userId, PagedRequest request, CancellationToken ct = default);

    Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default);

    Task MarkAsReadAsync(Guid userId, MarkReadDto dto, CancellationToken ct = default);

    Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default);

    Task DeleteNotificationAsync(Guid notificationId, Guid userId, CancellationToken ct = default);

    // Internal — called by other services to dispatch notifications
    Task NotifyNewMaterialAsync(Guid courseId, Guid materialId, CancellationToken ct = default);

    Task NotifyNewAssignmentAsync(Guid courseId, Guid assignmentId, CancellationToken ct = default);

    Task NotifyNewExamAsync(Guid courseId, Guid examId, CancellationToken ct = default);

    Task NotifyAssignmentGradedAsync(Guid studentId, Guid submissionId, CancellationToken ct = default);

    Task NotifyCourseUpdateAsync(Guid courseId, string messageAr, string messageEn, CancellationToken ct = default);

    Task SendGeneralNotificationAsync(Guid userId, string titleAr, string titleEn, string bodyAr, string bodyEn, CancellationToken ct = default);
}
