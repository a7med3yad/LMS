using LMS.DTOs.Users;
using LMS.Common.Pagination;

namespace LMS.Application.Services.Interfaces;

public interface IUserService
{
    Task<UserProfileDto> GetProfileAsync(Guid userId, CancellationToken ct = default);

    Task<UserProfileDto> UpdateProfileAsync(Guid userId, UpdateProfileDto dto, CancellationToken ct = default);

    Task<UserProfileDto> UpdateAvatarAsync(Guid userId, UpdateAvatarDto dto, CancellationToken ct = default);

    Task<PagedResult<UserSummaryDto>> GetAllUsersAsync(PagedRequest request, CancellationToken ct = default);

    Task<UserProfileDto> GetUserByIdAsync(Guid userId, CancellationToken ct = default);

    Task DeactivateUserAsync(Guid userId, CancellationToken ct = default);

    Task ActivateUserAsync(Guid userId, CancellationToken ct = default);

    Task<IEnumerable<UserSummaryDto>> GetInstructorsAsync(CancellationToken ct = default);
}
