using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Users;
using LMS.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Command.User;

public class UpdateAvatarHandler : IRequestHandler<UpdateAvatarCommand, UserProfileDto>
{
    private readonly UserManager<ApplicationUser> _um;
    public UpdateAvatarHandler(UserManager<ApplicationUser> um) => _um = um;

    public async Task<UserProfileDto> Handle(UpdateAvatarCommand req, CancellationToken ct)
    {
        var user = await _um.FindByIdAsync(req.UserId.ToString())
            ?? throw new NotFoundException("User", req.UserId);

        user.AvatarUrl = req.Dto.AvatarUrl;
        await _um.UpdateAsync(user);

        return new UserProfileDto(user.Id.ToString(), user.FullName, user.Email!,
            user.AvatarUrl, user.Role.ToString(), user.IsActive, user.IsVerified, user.CreatedAt);
    }
}
