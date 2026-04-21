using LMS.Domain.Common.Pagination;
using LMS.Domain.Models;
using LMS.Domain.Models.Enums;
using LMS.Infrastructure.Persistence;
using LMS.Infrastructure.Repositories;
using LMS.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LMS.Repositories;

public class UserRepository : GenericRepository<ApplicationUser>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context) { }

    public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken ct = default)
        => await _dbSet.FirstOrDefaultAsync(u => u.Email == email, ct);

    public async Task<PagedResult<ApplicationUser>> GetPagedUsersAsync(PagedRequest request, CancellationToken ct = default)
    {
        var query = _dbSet.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(u =>
                u.FullName.Contains(request.Search) ||
                u.Email!.Contains(request.Search));

        var totalCount = await query.CountAsync(ct);

        query = query.OrderByDescending(u => u.CreatedAt);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        return new PagedResult<ApplicationUser>
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersByRoleAsync(UserRole role, CancellationToken ct = default)
        => await _dbSet.Where(u => u.Role == role).ToListAsync(ct);

    public async Task<ApplicationUser?> GetUserWithEnrollmentsAsync(Guid userId, CancellationToken ct = default)
        => await _dbSet
            .Include(u => u.Enrollments)
            .FirstOrDefaultAsync(u => u.Id == userId, ct);
}
