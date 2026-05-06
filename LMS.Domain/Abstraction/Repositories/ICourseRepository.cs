
using LMS.Domain.DTOs.Courses;
using LMS.Domain.DTOs.Pagination;
using LMS.Domain.Models;

namespace LMS.Domain.Abstraction.Repositories;

public interface ICourseRepository : IRepository<Course>
{
    Task<Course?> GetCourseWithDetailsAsync(Guid courseId, CancellationToken ct = default);

    Task<PagedResult<Course>> GetPagedCoursesAsync(CourseFilterDto filter, CancellationToken ct = default);

    Task<IEnumerable<Course>> GetCoursesByInstructorAsync(Guid instructorId, CancellationToken ct = default);

    Task<IEnumerable<Course>> GetPublishedCoursesAsync(CancellationToken ct = default);

    Task<bool> IsInstructorOfCourseAsync(Guid instructorId, Guid courseId, CancellationToken ct = default);

    Task<Course?> GetCourseWithMaterialsAsync(Guid courseId, CancellationToken ct = default);
}
