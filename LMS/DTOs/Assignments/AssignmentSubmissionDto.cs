using LMS.DTOs.Users;

namespace LMS.DTOs.Assignments;

public record AssignmentSubmissionDto(
    Guid Id,
    Guid AssignmentId,
    string AssignmentTitle,
    UserSummaryDto Student,
    string? TextAnswer,
    string? FileUrl,
    int Grade,
    string? Feedback,
    DateTime SubmittedAt,
    DateTime? GradedAt
);
