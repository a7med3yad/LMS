using System.ComponentModel.DataAnnotations;

namespace LMS.DTOs.Payments;

public record ApplyVoucherDto(
    [Required] Guid CourseId,
    [Required] string VoucherCode
);
