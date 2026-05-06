using MediatR;

namespace LMS.Application.Command.Notification;
public record MarkReadCommand(Guid NotificationId, Guid UserId) : IRequest;
