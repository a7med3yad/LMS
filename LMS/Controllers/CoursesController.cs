using LMS.Application.Services.Interfaces;
using LMS.Domain.Common.Constants;
using LMS.Domain.Common.Pagination;
using LMS.Domain.DTOs.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService) => _courseService = courseService;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET api/v1/courses
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(PagedResult<CourseSummaryDto>), 200)]
    public async Task<IActionResult> GetCourses([FromQuery] CourseFilterDto filter, CancellationToken ct)
    {
        var result = await _courseService.GetCoursesAsync(filter, ct);
        return Ok(result);
    }

    // GET api/v1/courses/{id}
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(CourseDto), 200)]
    public async Task<IActionResult> GetCourse(Guid id, CancellationToken ct)
    {
        var result = await _courseService.GetCourseAsync(id, ct);
        return Ok(result);
    }

    // GET api/v1/courses/my  [Instructor]
    [HttpGet("my")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(IEnumerable<CourseSummaryDto>), 200)]
    public async Task<IActionResult> GetMyCourses(CancellationToken ct)
    {
        var result = await _courseService.GetMyCourseAsInstructorAsync(GetUserId(), ct);
        return Ok(result);
    }

    // GET api/v1/courses/enrolled  [Student]
    [HttpGet("enrolled")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(IEnumerable<CourseSummaryDto>), 200)]
    public async Task<IActionResult> GetEnrolledCourses(CancellationToken ct)
    {
        var result = await _courseService.GetEnrolledCoursesAsync(GetUserId(), ct);
        return Ok(result);
    }

    // POST api/v1/courses  [Instructor]
    [HttpPost]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(CourseDto), 201)]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto dto, CancellationToken ct)
    {
        var result = await _courseService.CreateCourseAsync(GetUserId(), dto, ct);
        return CreatedAtAction(nameof(GetCourse), new { id = result.Id }, result);
    }

    // PUT api/v1/courses/{id}  [Instructor]
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(CourseDto), 200)]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseDto dto, CancellationToken ct)
    {
        var result = await _courseService.UpdateCourseAsync(id, GetUserId(), dto, ct);
        return Ok(result);
    }

    // PATCH api/v1/courses/{id}/publish  [Instructor]
    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> PublishCourse(Guid id, CancellationToken ct)
    {
        await _courseService.PublishCourseAsync(id, GetUserId(), ct);
        return NoContent();
    }

    // PATCH api/v1/courses/{id}/archive  [Instructor/Admin]
    [HttpPatch("{id:guid}/archive")]
    [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> ArchiveCourse(Guid id, CancellationToken ct)
    {
        await _courseService.ArchiveCourseAsync(id, GetUserId(), ct);
        return NoContent();
    }

    // DELETE api/v1/courses/{id}  [Instructor/Admin]
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken ct)
    {
        await _courseService.DeleteCourseAsync(id, GetUserId(), ct);
        return NoContent();
    }
}
