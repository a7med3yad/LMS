using LMS.Application.Abstraction.UnitOfWork;
using MediatR;

namespace LMS.Application.Command.Notification;

public class MarkAllAsReadHandler : IRequestHandler<MarkAllAsReadCommand>
{
    private readonly IUnitOfWork _uow;
    public MarkAllAsReadHandler(IUnitOfWork uow) => _uow = uow;

    public async Task Handle(MarkAllAsReadCommand req, CancellationToken ct)
    {
        var all = await _uow.Notifications.FindAsync(
            n => n.UserId == req.UserId && !n.IsRead, ct);

        foreach (var n in all) { n.IsRead = true; _uow.Notifications.Update(n); }
        await _uow.SaveChangesAsync(ct);
    }
}
