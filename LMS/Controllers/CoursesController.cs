using LMS.Application.Command.Course;
using LMS.Application.DTOs.Courses;
using LMS.Application.Query.Course;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/courses")]
public class CoursesController : ControllerBase
{
    private readonly ISender _sender;
    public CoursesController(ISender sender) => _sender = sender;

    private Guid UserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetCourses([FromQuery] CourseFilterDto filter, CancellationToken ct)
        => Ok(await _sender.Send(new GetCoursesQuery(filter), ct));

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCourse(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetCourseQuery(id), ct));

    [HttpGet("my")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> GetMyCourses(CancellationToken ct)
        => Ok(await _sender.Send(new GetMyCoursesQuery(UserId()), ct));

    [HttpGet("enrolled")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetEnrolledCourses(CancellationToken ct)
        => Ok(await _sender.Send(new GetEnrolledCoursesQuery(UserId()), ct));

    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Create(CreateCourseDto dto, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateCourseCommand(UserId(), dto), ct);
        return CreatedAtAction(nameof(GetCourse), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Update(Guid id, UpdateCourseDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new UpdateCourseCommand(id, UserId(), dto), ct));

    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
    {
        await _sender.Send(new PublishCourseCommand(id, UserId()), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/archive")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct)
    {
        await _sender.Send(new ArchiveCourseCommand(id, UserId()), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeleteCourseCommand(id, UserId()), ct);
        return NoContent();
    }
}
