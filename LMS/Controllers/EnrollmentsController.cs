using LMS.Application.Services.Interfaces;
using LMS.Domain.Common.Constants;
using LMS.Domain.DTOs.Enrollments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService) => _enrollmentService = enrollmentService;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // POST api/v1/enrollments  [Student]
    [HttpPost]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(EnrollmentDto), 201)]
    public async Task<IActionResult> Enroll([FromBody] EnrollRequestDto dto, CancellationToken ct)
    {
        var result = await _enrollmentService.EnrollAsync(GetUserId(), dto, ct);
        return CreatedAtAction(nameof(GetEnrollment), new { id = result.Id }, result);
    }

    // GET api/v1/enrollments/my  [Student]
    [HttpGet("my")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(IEnumerable<EnrollmentDto>), 200)]
    public async Task<IActionResult> GetMyEnrollments(CancellationToken ct)
    {
        var result = await _enrollmentService.GetMyEnrollmentsAsync(GetUserId(), ct);
        return Ok(result);
    }

    // GET api/v1/enrollments/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EnrollmentDto), 200)]
    public async Task<IActionResult> GetEnrollment(Guid id, CancellationToken ct)
    {
        var result = await _enrollmentService.GetEnrollmentAsync(id, GetUserId(), ct);
        return Ok(result);
    }

    // GET api/v1/enrollments/course/{courseId}  [Instructor/Admin]
    [HttpGet("course/{courseId:guid}")]
    [Authorize(Roles = $"{Roles.Instructor},{Roles.Admin}")]
    [ProducesResponseType(typeof(IEnumerable<EnrollmentDto>), 200)]
    public async Task<IActionResult> GetCourseEnrollments(Guid courseId, CancellationToken ct)
    {
        var result = await _enrollmentService.GetCourseEnrollmentsAsync(courseId, GetUserId(), ct);
        return Ok(result);
    }

    // PATCH api/v1/enrollments/{id}/complete  [Student]
    [HttpPatch("{id:guid}/complete")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> CompleteEnrollment(Guid id, CancellationToken ct)
    {
        await _enrollmentService.CompleteEnrollmentAsync(id, GetUserId(), ct);
        return NoContent();
    }

    // PATCH api/v1/enrollments/{id}/suspend  [Admin]
    [HttpPatch("{id:guid}/suspend")]
    [Authorize(Roles = Roles.Admin)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> SuspendEnrollment(Guid id, CancellationToken ct)
    {
        await _enrollmentService.SuspendEnrollmentAsync(id, GetUserId(), ct);
        return NoContent();
    }
}
