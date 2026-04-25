using LMS.Application.Abstraction.UnitOfWork;
using MediatR;

namespace LMS.Application.Command.Notification;

public class MarkAsReadHandler : IRequestHandler<MarkAsReadCommand>
{
    private readonly IUnitOfWork _uow;
    public MarkAsReadHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(MarkAsReadCommand req, CancellationToken ct)
    {
        foreach (var id in req.Dto.NotificationIds)
        {
            var n = await _uow.Notifications.GetByIdAsync(id, ct);
            if (n is null || n.UserId != req.UserId) continue;
            n.IsRead = true;
            _uow.Notifications.Update(n);
        }
        await _uow.SaveChangesAsync(ct);
    }
}
