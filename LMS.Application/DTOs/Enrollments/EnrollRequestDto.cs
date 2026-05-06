using System.ComponentModel.DataAnnotations;

<<<<<<< HEAD
namespace LMS.Application.DTOs.Enrollments;
=======
namespace LMS.Domain.DTOs.Enrollments;
>>>>>>> 4450aad95aa0059499e5c99c961c831b227af253

public record EnrollRequestDto(
    [Required] Guid CourseId,
    string? VoucherCode,
    string? PaymentIntentId
);
