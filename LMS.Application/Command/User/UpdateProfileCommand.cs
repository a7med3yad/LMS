using LMS.Application.DTOs.Users;
using MediatR;

namespace LMS.Application.Command.User;

public record UpdateProfileCommand(Guid UserId, UpdateProfileDto Dto) : IRequest<UserProfileDto>;
