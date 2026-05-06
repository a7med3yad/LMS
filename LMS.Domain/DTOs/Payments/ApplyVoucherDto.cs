using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.DTOs.Payments;

public record ApplyVoucherDto(
    [Required] Guid CourseId,
    [Required] string VoucherCode
);
