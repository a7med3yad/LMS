using MediatR;

namespace LMS.Application.Command.User;

public record ActivateUserCommand(Guid UserId) : IRequest;
