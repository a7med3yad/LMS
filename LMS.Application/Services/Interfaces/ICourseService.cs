
using LMS.Domain.Common.Pagination;
using LMS.Domain.DTOs.Courses;

namespace LMS.Application.Services.Interfaces;

public interface ICourseService
{
    Task<CourseDto> GetCourseAsync(Guid courseId, CancellationToken ct = default);

    Task<PagedResult<CourseSummaryDto>> GetCoursesAsync(CourseFilterDto filter, CancellationToken ct = default);

    Task<IEnumerable<CourseSummaryDto>> GetMyCourseAsInstructorAsync(Guid instructorId, CancellationToken ct = default);

    Task<IEnumerable<CourseSummaryDto>> GetEnrolledCoursesAsync(Guid studentId, CancellationToken ct = default);

    Task<CourseDto> CreateCourseAsync(Guid instructorId, CreateCourseDto dto, CancellationToken ct = default);

    Task<CourseDto> UpdateCourseAsync(Guid courseId, Guid requesterId, UpdateCourseDto dto, CancellationToken ct = default);

    Task PublishCourseAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);

    Task ArchiveCourseAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);

    Task DeleteCourseAsync(Guid courseId, Guid requesterId, CancellationToken ct = default);
}
