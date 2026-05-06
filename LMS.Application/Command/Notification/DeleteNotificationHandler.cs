using LMS.Application.Abstraction.UnitOfWork;
using LMS.Application.Common.Exceptions;
using MediatR;

namespace LMS.Application.Command.Notification;

public class DeleteNotificationHandler : IRequestHandler<MarkReadCommand>
{
    private readonly IUnitOfWork _uow;
    public DeleteNotificationHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(MarkReadCommand req, CancellationToken ct)
    {
        var n = await _uow.Notifications.GetByIdAsync(req.NotificationId, ct)
            ?? throw new NotFoundException("Notification", req.NotificationId);

        if (n.UserId != req.UserId) throw new ForbiddenException();

        _uow.Notifications.Remove(n);
        await _uow.SaveChangesAsync(ct);
    }
}