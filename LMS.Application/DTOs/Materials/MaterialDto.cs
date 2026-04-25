using LMS.Domain.Models.Enums;

namespace LMS.Application.DTOs.Materials;

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
