using MediatR;

namespace LMS.Application.Command.Notification;

public record MarkAllAsReadCommand(Guid UserId) : IRequest;
