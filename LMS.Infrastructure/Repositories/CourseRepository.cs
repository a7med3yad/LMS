using LMS.Domain.Common.Pagination;
using LMS.Domain.DTOs.Courses;
using LMS.Domain.Models;
using LMS.Domain.Repositories.Interfaces;
using LMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LMS.Infrastructure.Repositories;

public class CourseRepository : GenericRepository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context) { }

    public async Task<Course?> GetCourseWithDetailsAsync(Guid courseId, CancellationToken ct = default)
        => await _dbSet
            .Include(c => c.Instructor)
            .Include(c => c.Materials)
            .Include(c => c.Enrollments)
            .FirstOrDefaultAsync(c => c.Id == courseId, ct);

    public async Task<PagedResult<Course>> GetPagedCoursesAsync(CourseFilterDto filter, CancellationToken ct = default)
    {
        var query = _dbSet.Include(c => c.Instructor).Include(c => c.Enrollments).AsQueryable();

        if (filter.Status.HasValue)
            query = query.Where(c => c.Status == filter.Status.Value);

        if (filter.InstructorId.HasValue)
            query = query.Where(c => c.InstructorId == filter.InstructorId.Value);

        if (filter.MinPrice.HasValue)
            query = query.Where(c => c.Price >= filter.MinPrice.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(c => c.Price <= filter.MaxPrice.Value);

        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(c =>
                c.TitleAr.Contains(filter.Search) ||
                c.TitleEn.Contains(filter.Search));

        var totalCount = await query.CountAsync(ct);

        if (!string.IsNullOrWhiteSpace(filter.SortBy))
        {
            query = filter.SortBy.ToLower() switch
            {
                "price" => filter.SortDescending ? query.OrderByDescending(c => c.Price) : query.OrderBy(c => c.Price),
                "createdat" => filter.SortDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
                "title" => filter.SortDescending ? query.OrderByDescending(c => c.TitleEn) : query.OrderBy(c => c.TitleEn),
                _ => query.OrderByDescending(c => c.CreatedAt)
            };
        }
        else
        {
            query = query.OrderByDescending(c => c.CreatedAt);
        }

        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(ct);

        return new PagedResult<Course>
        {
            Items = items,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<IEnumerable<Course>> GetCoursesByInstructorAsync(Guid instructorId, CancellationToken ct = default)
        => await _dbSet
            .Include(c => c.Enrollments)
            .Where(c => c.InstructorId == instructorId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

    public async Task<IEnumerable<Course>> GetPublishedCoursesAsync(CancellationToken ct = default)
        => await _dbSet
            .Include(c => c.Instructor)
            .Include(c => c.Enrollments)
            .Where(c => c.Status == Domain.Models.Enums.CourseStatus.Published)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

    public async Task<bool> IsInstructorOfCourseAsync(Guid instructorId, Guid courseId, CancellationToken ct = default)
        => await _dbSet.AnyAsync(c => c.Id == courseId && c.InstructorId == instructorId, ct);

    public async Task<Course?> GetCourseWithMaterialsAsync(Guid courseId, CancellationToken ct = default)
        => await _dbSet
            .Include(c => c.Materials.OrderBy(m => m.Order))
            .FirstOrDefaultAsync(c => c.Id == courseId, ct);
}
