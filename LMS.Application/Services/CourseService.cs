using LMS.Application.Services.Interfaces;
using LMS.Domain.Common.Pagination;
using LMS.Domain.DTOs.Courses;

namespace LMS.Application.Services;

public class CourseService : ICourseService
{
    public Task<CourseDto> GetCourseAsync(Guid courseId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<PagedResult<CourseSummaryDto>> GetCoursesAsync(CourseFilterDto filter, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IEnumerable<CourseSummaryDto>> GetMyCourseAsInstructorAsync(Guid instructorId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<IEnumerable<CourseSummaryDto>> GetEnrolledCoursesAsync(Guid studentId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<CourseDto> CreateCourseAsync(Guid instructorId, CreateCourseDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task<CourseDto> UpdateCourseAsync(Guid courseId, Guid requesterId, UpdateCourseDto dto, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task PublishCourseAsync(Guid courseId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task ArchiveCourseAsync(Guid courseId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();

    public Task DeleteCourseAsync(Guid courseId, Guid requesterId, CancellationToken ct = default)
        => throw new NotImplementedException();
}
