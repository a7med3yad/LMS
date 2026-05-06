using LMS.Application.Common.Exceptions;
using LMS.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Command.User;

public class ActivateUserHandler : IRequestHandler<ActivateUserCommand>
{
    private readonly UserManager<ApplicationUser> _um;
    public ActivateUserHandler(UserManager<ApplicationUser> um) => _um = um;

    public async Task Handle(ActivateUserCommand req, CancellationToken ct)
    {
        var user = await _um.FindByIdAsync(req.UserId.ToString())
            ?? throw new NotFoundException("User", req.UserId);
        user.IsActive = true;
        await _um.UpdateAsync(user);
    }
}