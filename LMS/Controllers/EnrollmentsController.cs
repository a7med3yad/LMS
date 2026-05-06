using LMS.Application.Command.Enrollment;
using LMS.Application.DTOs.Enrollments;
using LMS.Application.Query.Enrollment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/enrollments")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly ISender _sender;
    public EnrollmentsController(ISender sender) => _sender = sender;

    private Guid UserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpPost]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Enroll(EnrollRequestDto dto, CancellationToken ct)
    {
        var result = await _sender.Send(new EnrollCommand(UserId(), dto), ct);
        return CreatedAtAction(nameof(GetEnrollment), new { id = result.Id }, result);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyEnrollments(CancellationToken ct)
        => Ok(await _sender.Send(new GetMyEnrollmentsQuery(UserId()), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEnrollment(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetEnrollmentQuery(id, UserId()), ct));

    [HttpGet("course/{courseId:guid}")]
    [Authorize(Roles = "Instructor,Admin")]
    public async Task<IActionResult> GetCourseEnrollments(Guid courseId, CancellationToken ct)
        => Ok(await _sender.Send(new GetCourseEnrollmentsQuery(courseId, UserId()), ct));

    [HttpPatch("{id:guid}/complete")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken ct)
    {
        await _sender.Send(new CompleteEnrollmentCommand(id, UserId()), ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/suspend")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Suspend(Guid id, CancellationToken ct)
    {
        await _sender.Send(new SuspendEnrollmentCommand(id, UserId()), ct);
        return NoContent();
    }
}
