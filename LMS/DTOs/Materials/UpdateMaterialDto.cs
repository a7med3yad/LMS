using System.ComponentModel.DataAnnotations;
using LMS.Models.Enums;

namespace LMS.DTOs.Materials;

public record UpdateMaterialDto(
    [MaxLength(300)] string? TitleAr,
    [MaxLength(300)] string? TitleEn,
    [MaxLength(2000)] string? DescriptionAr,
    [MaxLength(2000)] string? DescriptionEn,
    MaterialType? Type,
    string? ContentUrl,
    string? TextContent,
    int? Order,
    bool? IsPublished
);
