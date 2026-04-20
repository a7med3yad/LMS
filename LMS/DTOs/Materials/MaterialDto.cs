using LMS.Models.Enums;

namespace LMS.DTOs.Materials;

public record MaterialDto(
    Guid Id,
    string TitleAr,
    string TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    MaterialType Type,
    string ContentUrl,
    string? TextContent,
    int Order,
    bool IsPublished,
    Guid CourseId,
    DateTime CreatedAt
);
