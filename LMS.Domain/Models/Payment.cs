using LMS.Domain.Models.Enums;

namespace LMS.Domain.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        public string Provider { get; set; } = string.Empty;
        public string? ProviderTransactionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }

        // FK
        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;

        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;
    }
}
