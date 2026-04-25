using MediatR;

namespace LMS.Application.Command.User;

public record DeactivateUserCommand(Guid UserId) : IRequest;
