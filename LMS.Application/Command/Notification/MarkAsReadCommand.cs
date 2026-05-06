using LMS.Application.DTOs.Notifications;
using MediatR;

namespace LMS.Application.Command.Notification;

public record MarkAsReadCommand(Guid UserId, MarkReadDto Dto) : IRequest;
