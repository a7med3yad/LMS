namespace LMS.Domain.DTOs.Assignments;

public record SubmitAssignmentDto(
    string? TextAnswer,
    string? FileUrl
);
