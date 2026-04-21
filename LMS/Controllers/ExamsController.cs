using LMS.Application.Services.Interfaces;
using LMS.Domain.Common.Constants;
using LMS.Domain.DTOs.Exams;
using LMS.DTOs.Exams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LMS.Api.Controllers;

[ApiController]
[Route("api/v1/courses/{courseId:guid}/exams")]
[Authorize]
public class ExamsController : ControllerBase
{
    private readonly IExamService _examService;

    public ExamsController(IExamService examService) => _examService = examService;

    private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExamDto>), 200)]
    public async Task<IActionResult> GetExams(Guid courseId, CancellationToken ct)
    {
        var result = await _examService.GetCourseExamsAsync(courseId, GetUserId(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ExamDto), 200)]
    public async Task<IActionResult> GetExam(Guid courseId, Guid id, CancellationToken ct)
    {
        var result = await _examService.GetExamAsync(id, GetUserId(), ct);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(ExamDto), 201)]
    public async Task<IActionResult> CreateExam(Guid courseId, [FromBody] CreateExamDto dto, CancellationToken ct)
    {
        var result = await _examService.CreateExamAsync(courseId, GetUserId(), dto, ct);
        return CreatedAtAction(nameof(GetExam), new { courseId, id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(ExamDto), 200)]
    public async Task<IActionResult> UpdateExam(Guid courseId, Guid id, [FromBody] UpdateExamDto dto, CancellationToken ct)
    {
        var result = await _examService.UpdateExamAsync(id, GetUserId(), dto, ct);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/publish")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> PublishExam(Guid courseId, Guid id, CancellationToken ct)
    {
        await _examService.PublishExamAsync(id, GetUserId(), ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteExam(Guid courseId, Guid id, CancellationToken ct)
    {
        await _examService.DeleteExamAsync(id, GetUserId(), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/questions")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(QuestionDto), 201)]
    public async Task<IActionResult> AddQuestion(Guid courseId, Guid id, [FromBody] CreateQuestionDto dto, CancellationToken ct)
    {
        var result = await _examService.AddQuestionAsync(id, GetUserId(), dto, ct);
        return Created("", result);
    }

    [HttpPut("{id:guid}/questions/{questionId:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(QuestionDto), 200)]
    public async Task<IActionResult> UpdateQuestion(Guid courseId, Guid id, Guid questionId, [FromBody] UpdateQuestionDto dto, CancellationToken ct)
    {
        var result = await _examService.UpdateQuestionAsync(questionId, GetUserId(), dto, ct);
        return Ok(result);
    }

    [HttpDelete("{id:guid}/questions/{questionId:guid}")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> DeleteQuestion(Guid courseId, Guid id, Guid questionId, CancellationToken ct)
    {
        await _examService.DeleteQuestionAsync(questionId, GetUserId(), ct);
        return NoContent();
    }

    [HttpPost("{id:guid}/start")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(StartExamResponseDto), 200)]
    public async Task<IActionResult> StartExam(Guid courseId, Guid id, CancellationToken ct)
    {
        var result = await _examService.StartExamAsync(id, GetUserId(), ct);
        return Ok(result);
    }

    [HttpPost("{id:guid}/submit")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(ExamAttemptDto), 200)]
    public async Task<IActionResult> SubmitExam(Guid courseId, Guid id, [FromBody] SubmitExamDto dto, CancellationToken ct)
    {
        var result = await _examService.SubmitExamAsync(dto, GetUserId(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}/attempts/my")]
    [Authorize(Roles = Roles.Student)]
    [ProducesResponseType(typeof(IEnumerable<ExamAttemptDto>), 200)]
    public async Task<IActionResult> GetMyAttempts(Guid courseId, Guid id, CancellationToken ct)
    {
        var result = await _examService.GetMyAttemptsAsync(id, GetUserId(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}/attempts")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(typeof(IEnumerable<ExamAttemptDto>), 200)]
    public async Task<IActionResult> GetAllAttempts(Guid courseId, Guid id, CancellationToken ct)
    {
        var result = await _examService.GetAllAttemptsAsync(id, GetUserId(), ct);
        return Ok(result);
    }

    [HttpGet("attempts/{attemptId:guid}")]
    [ProducesResponseType(typeof(ExamAttemptDto), 200)]
    public async Task<IActionResult> GetAttemptResult(Guid courseId, Guid attemptId, CancellationToken ct)
    {
        var result = await _examService.GetAttemptResultAsync(attemptId, GetUserId(), ct);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/grade-open")]
    [Authorize(Roles = Roles.Instructor)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GradeOpenEndedAnswer(Guid courseId, Guid id, [FromBody] GradeOpenEndedDto dto, CancellationToken ct)
    {
        await _examService.GradeOpenEndedAnswerAsync(id, GetUserId(), dto, ct);
        return NoContent();
    }
}
