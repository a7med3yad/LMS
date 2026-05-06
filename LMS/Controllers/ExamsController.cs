using LMS.Application.Command.Exam;
using LMS.Application.DTOs.Exams;
using LMS.Application.Query.Exam;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/exams")]
[Authorize]
public class ExamsController : ControllerBase
{
    private readonly ISender _sender;
    public ExamsController(ISender sender) => _sender = sender;

    private Guid UserId() =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetExams(Guid courseId, CancellationToken ct)
        => Ok(await _sender.Send(new GetCourseExamsQuery(courseId, UserId()), ct));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetExam(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetExamQuery(id, UserId()), ct));

    [HttpPost]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Create(Guid courseId, CreateExamDto dto, CancellationToken ct)
    {
        var result = await _sender.Send(new CreateExamCommand(courseId, UserId(), dto), ct);
        return CreatedAtAction(nameof(GetExam), new { courseId, id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Update(Guid id, UpdateExamDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new UpdateExamCommand(id, UserId(), dto), ct));

    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Publish(Guid id, CancellationToken ct)
    {
        await _sender.Send(new PublishExamCommand(id, UserId()), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _sender.Send(new DeleteExamCommand(id, UserId()), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/questions")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> AddQuestion(Guid id, CreateQuestionDto dto, CancellationToken ct)
    {
        var result = await _sender.Send(new AddQuestionCommand(id, UserId(), dto), ct);
        return CreatedAtAction(nameof(GetExam), new { courseId = Guid.Empty, id }, result);
    }

    [HttpPut("{id:guid}/questions/{questionId:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> UpdateQuestion(Guid questionId, UpdateQuestionDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new UpdateQuestionCommand(questionId, UserId(), dto), ct));

    [HttpDelete("{id:guid}/questions/{questionId:guid}")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> DeleteQuestion(Guid questionId, CancellationToken ct)
    {
        await _sender.Send(new DeleteQuestionCommand(questionId, UserId()), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/start")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> StartExam(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new StartExamCommand(id, UserId()), ct));

    [HttpPost("{id:guid}/submit")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> SubmitExam(SubmitExamDto dto, CancellationToken ct)
        => Ok(await _sender.Send(new SubmitExamCommand(dto, UserId()), ct));

    [HttpGet("{id:guid}/attempts/my")]
    [Authorize(Roles = "Student")]
    public async Task<IActionResult> GetMyAttempts(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetMyAttemptsQuery(id, UserId()), ct));

    [HttpGet("{id:guid}/attempts")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> GetAllAttempts(Guid id, CancellationToken ct)
        => Ok(await _sender.Send(new GetAllAttemptsQuery(id, UserId()), ct));

    [HttpGet("attempts/{attemptId:guid}")]
    public async Task<IActionResult> GetAttemptResult(Guid attemptId, CancellationToken ct)
        => Ok(await _sender.Send(new GetAttemptResultQuery(attemptId, UserId()), ct));

    [HttpPatch("{id:guid}/grade-open")]
    [Authorize(Roles = "Instructor")]
    public async Task<IActionResult> GradeOpenEnded(Guid id, GradeOpenEndedDto dto, CancellationToken ct)
    {
        await _sender.Send(new GradeOpenEndedCommand(id, UserId(), dto), ct);
        return NoContent();
    }
}
