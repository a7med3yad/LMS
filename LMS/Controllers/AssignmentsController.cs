using LMS.Application.Services.Interfaces;
using LMS.Common.Constants;
using LMS.DTOs.Assignments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/assignments")]
[Authorize]
public class AssignmentsController : ControllerBase
{
    private readonly IAssignmentService _assignmentService;

    public AssignmentsController(IAssignmentService assignmentService) => _assignmentService = assignmentService;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // GET api/v1/courses/{courseId}/assignments
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AssignmentDto>), 200)]
    public async Task<IActionResult> GetAssignments(Guid courseId, CancellationToken ct)
    {
        var result = await _assignmentService.GetCourseAssignmentsAsync(courseId, GetUserId(), ct);
        return Ok(result);
    }

    // GET api/v1/courses/{courseId}/assignments/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AssignmentDto), 200)]
    public async Task<IActionResult> GetAssignment(Guid courseId, Guid id, CancellationToken ct)
    {
        var result = await _assignmentService.GetAssignmentAsync(id, GetUserId(), ct);
        return Ok(result);
    }

    // POST api/v1/courses/{courseId}/assignments  [Instructor]
    [HttpPost]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(AssignmentDto), 201)]
    public async Task<IActionResult> CreateAssignment(Guid courseId, [FromBody] CreateAssignmentDto dto, CancellationToken ct)
    {
        var result = await _assignmentService.CreateAssignmentAsync(courseId, GetUserId(), dto, ct);
        return CreatedAtAction(nameof(GetAssignment), new { courseId, id = result.Id }, result);
    }

    // PUT api/v1/courses/{courseId}/assignments/{id}  [Instructor]
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(AssignmentDto), 200)]
    public async Task<IActionResult> UpdateAssignment(Guid courseId, Guid id, [FromBody] UpdateAssignmentDto dto, CancellationToken ct)
    {
        var result = await _assignmentService.UpdateAssignmentAsync(id, GetUserId(), dto, ct);
        return Ok(result);
    }

    // PATCH api/v1/courses/{courseId}/assignments/{id}/publish  [Instructor]
    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> PublishAssignment(Guid courseId, Guid id, CancellationToken ct)
    {
        await _assignmentService.PublishAssignmentAsync(id, GetUserId(), ct);
        return NoContent();
    }

    // DELETE api/v1/courses/{courseId}/assignments/{id}  [Instructor]
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteAssignment(Guid courseId, Guid id, CancellationToken ct)
    {
        await _assignmentService.DeleteAssignmentAsync(id, GetUserId(), ct);
        return NoContent();
    }

    // POST api/v1/courses/{courseId}/assignments/{id}/submit  [Student]
    [HttpPost("{id:guid}/submit")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(AssignmentSubmissionDto), 200)]
    public async Task<IActionResult> SubmitAssignment(Guid courseId, Guid id, [FromBody] SubmitAssignmentDto dto, CancellationToken ct)
    {
        var result = await _assignmentService.SubmitAssignmentAsync(id, GetUserId(), dto, ct);
        return Ok(result);
    }

    // GET api/v1/courses/{courseId}/assignments/{id}/my-submission  [Student]
    [HttpGet("{id:guid}/my-submission")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(AssignmentSubmissionDto), 200)]
    public async Task<IActionResult> GetMySubmission(Guid courseId, Guid id, CancellationToken ct)
    {
        var result = await _assignmentService.GetMySubmissionAsync(id, GetUserId(), ct);
        return Ok(result);
    }

    // GET api/v1/courses/{courseId}/assignments/{id}/submissions  [Instructor]
    [HttpGet("{id:guid}/submissions")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(IEnumerable<AssignmentSubmissionDto>), 200)]
    public async Task<IActionResult> GetSubmissions(Guid courseId, Guid id, CancellationToken ct)
    {
        var result = await _assignmentService.GetSubmissionsAsync(id, GetUserId(), ct);
        return Ok(result);
    }

    // PATCH api/v1/courses/{courseId}/assignments/submissions/{submissionId}/grade  [Instructor]
    [HttpPatch("submissions/{submissionId:guid}/grade")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(AssignmentSubmissionDto), 200)]
    public async Task<IActionResult> GradeSubmission(Guid courseId, Guid submissionId, [FromBody] GradeSubmissionDto dto, CancellationToken ct)
    {
        var result = await _assignmentService.GradeSubmissionAsync(submissionId, GetUserId(), dto, ct);
        return Ok(result);
    }
}
