using System.ComponentModel.DataAnnotations;
using LMS.Models.Enums;

namespace LMS.DTOs.Materials;

public record CreateMaterialDto(
    [Required][MaxLength(300)] string TitleAr,
    [Required][MaxLength(300)] string TitleEn,
    [MaxLength(2000)] string? DescriptionAr,
    [MaxLength(2000)] string? DescriptionEn,
    [Required] MaterialType Type,
    [Required] string ContentUrl,
    string? TextContent,
    int Order
);
