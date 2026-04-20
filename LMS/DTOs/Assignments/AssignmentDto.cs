using LMS.Models.Enums;

namespace LMS.DTOs.Assignments;

public record AssignmentDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string DescriptionAr,
    string DescriptionEn,
    SubmissionType SubmissionType,
    DateTime DeadLine,
    int MaxGrade,
    bool IsPublished,
    Guid CourseId,
    DateTime CreatedAt
);
