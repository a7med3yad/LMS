using LMS.Application.Services.Interfaces;
using LMS.Domain.Common.Pagination;
using LMS.Domain.DTOs.Notifications;

namespace LMS.Application.Services;

public class NotificationService : INotificationService
{
    public Task<PagedResult<NotificationDto>> GetMyNotificationsAsync(Guid userId, PagedRequest request, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<int> GetUnreadCountAsync(Guid userId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task MarkAsReadAsync(Guid userId, MarkReadDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task MarkAllAsReadAsync(Guid userId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeleteNotificationAsync(Guid notificationId, Guid userId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task NotifyNewMaterialAsync(Guid courseId, Guid materialId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task NotifyNewAssignmentAsync(Guid courseId, Guid assignmentId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task NotifyNewExamAsync(Guid courseId, Guid examId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task NotifyAssignmentGradedAsync(Guid studentId, Guid submissionId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task NotifyCourseUpdateAsync(Guid courseId, string messageAr, string messageEn, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task SendGeneralNotificationAsync(Guid userId, string titleAr, string titleEn, string bodyAr, string bodyEn, CancellationToken ct = default)
        => throw new NotImplementedException();
}
