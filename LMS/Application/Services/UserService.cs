using LMS.Application.Services.Interfaces;
using LMS.Common.Pagination;
using LMS.DTOs.Users;

namespace LMS.Application.Services;

public class UserService : IUserService
{
    public Task<UserProfileDto> GetProfileAsync(Guid userId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<UserProfileDto> UpdateAvatarAsync(Guid userId, UpdateAvatarDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<PagedResult<UserSummaryDto>> GetAllUsersAsync(PagedRequest request, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<UserProfileDto> GetUserByIdAsync(Guid userId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeactivateUserAsync(Guid userId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task ActivateUserAsync(Guid userId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IEnumerable<UserSummaryDto>> GetInstructorsAsync(CancellationToken ct = default)
        => throw new NotImplementedException();
}
