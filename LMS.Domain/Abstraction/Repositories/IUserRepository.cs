using LMS.Domain.DTOs.Pagination;
using LMS.Domain.Models;
using LMS.Domain.Models.Enums;

namespace LMS.Domain.Abstraction.Repositories;

public interface IUserRepository : IRepository<ApplicationUser>
{
    Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default);

    Task<PagedResult<ApplicationUser>> GetPagedUsersAsync(PagedRequest request, CancellationToken ct = default);

    Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(UserRole role, CancellationToken ct = default);

    Task<ApplicationUser?> GetUserWithEnrollmentsAsync(Guid userId, CancellationToken ct = default);
}
