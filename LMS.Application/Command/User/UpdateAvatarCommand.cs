using LMS.Application.DTOs.Users;
using MediatR;

namespace LMS.Application.Command.User;

public record UpdateAvatarCommand(Guid UserId, UpdateAvatarDto Dto) : IRequest<UserProfileDto>;
