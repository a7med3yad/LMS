using LMS.Application.Command.Assignment;
using LMS.Application.DTOs.Assignments;
using LMS.Application.Query.Assignment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/assignments")]
[Authorize]
public class AssignmentsController : ControllerBase
{
    private readonly ISender _sender;
    public AssignmentsController(ISender sender) => _sender = sender;

    private Guid UserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAssignments(Guid courseId, CancellationToken ct)
        => Ok(await _sender.Send(new GetCourseAssignmentsQuery(courseId, UserId()), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetAssignment(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetAssignmentQuery(id, UserId()), ct));

    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Create(Guid courseId, CreateAssignmentDto dto, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateAssignmentCommand(courseId, UserId(), dto), ct);
        return CreatedAtAction(nameof(GetAssignment), new { courseId, id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Update(Guid id, UpdateAssignmentDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new UpdateAssignmentCommand(id, UserId(), dto), ct));

    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
    {
        await _sender.Send(new PublishAssignmentCommand(id, UserId()), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeleteAssignmentCommand(id, UserId()), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/submit")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> Submit(Guid id, SubmitAssignmentDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new SubmitAssignmentCommand(id, UserId(), dto), ct));

    [HttpGet("{id:guid}/my-submission")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMySubmission(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetMySubmissionQuery(id, UserId()), ct));

    [HttpGet("{id:guid}/submissions")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> GetSubmissions(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetSubmissionsQuery(id, UserId()), ct));

    [HttpPatch("submissions/{submissionId:guid}/grade")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> GradeSubmission(Guid submissionId, GradeSubmissionDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new GradeSubmissionCommand(submissionId, UserId(), dto), ct));
}
