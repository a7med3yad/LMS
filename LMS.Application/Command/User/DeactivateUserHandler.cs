using LMS.Application.Common.Exceptions;
using LMS.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Command.User;

public class DeactivateUserHandler : IRequestHandler<DeactivateUserCommand>
{
    private readonly UserManager<ApplicationUser> _um;
    public DeactivateUserHandler(UserManager<ApplicationUser> um) => _um = um;

    public async Task Handle(DeactivateUserCommand req, CancellationToken ct)
    {
        var user = await _um.FindByIdAsync(req.UserId.ToString())
            ?? throw new NotFoundException("User", req.UserId);
        user.IsActive = false;
        await _um.UpdateAsync(user);
    }
}
