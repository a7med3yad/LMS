using LMS.Common.Pagination;
using LMS.DTOs.Courses;
using LMS.Entities;

namespace LMS.Repositories.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    Task<Course?> GetCourseWithDetailsAsync(Guid courseId, CancellationToken ct = default);

    Task<PagedResult<Course>> GetPagedCoursesAsync(CourseFilterDto filter, CancellationToken ct = default);

    Task<IEnumerable<Course>> GetCoursesByInstructorAsync(Guid instructorId, CancellationToken ct = default);

    Task<IEnumerable<Course>> GetPublishedCoursesAsync(CancellationToken ct = default);

    Task<bool> IsInstructorOfCourseAsync(Guid instructorId, Guid courseId, CancellationToken ct = default);

    Task<Course?> GetCourseWithMaterialsAsync(Guid courseId, CancellationToken ct = default);
}
