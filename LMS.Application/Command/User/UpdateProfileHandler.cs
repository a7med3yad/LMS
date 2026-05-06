using LMS.Application.Common.Exceptions;
using LMS.Application.DTOs.Users;
using LMS.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Command.User;

public class UpdateProfileHandler : IRequestHandler<UpdateProfileCommand, UserProfileDto>
{
    private readonly UserManager<ApplicationUser> _um;
    public UpdateProfileHandler(UserManager<ApplicationUser> um) => _um = um;

    public async Task<UserProfileDto> Handle(UpdateProfileCommand req, CancellationToken ct)
    {
        var user = await _um.FindByIdAsync(req.UserId.ToString())
            ?? throw new NotFoundException("User", req.UserId);

        user.FullName = req.Dto.FullName;
        await _um.UpdateAsync(user);

        return new UserProfileDto(user.Id.ToString(), user.FullName, user.Email!,
            user.AvatarUrl, user.Role.ToString(), user.IsActive, user.IsVerified, user.CreatedAt);
    }
}
