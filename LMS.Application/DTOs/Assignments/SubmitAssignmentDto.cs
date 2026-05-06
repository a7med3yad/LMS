namespace LMS.Application.DTOs.Assignments;

public record SubmitAssignmentDto(
    string? TextAnswer,
    string? FileUrl
);
