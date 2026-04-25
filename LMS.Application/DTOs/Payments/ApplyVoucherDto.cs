using System.ComponentModel.DataAnnotations;

namespace LMS.Application.DTOs.Payments;

public record ApplyVoucherDto(
    [Required] Guid CourseId,
    [Required] string VoucherCode
);
