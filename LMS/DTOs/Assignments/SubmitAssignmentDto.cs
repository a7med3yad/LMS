namespace LMS.DTOs.Assignments;

public record SubmitAssignmentDto(
    string? TextAnswer,
    string? FileUrl
);
