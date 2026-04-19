using LMS.Models;
using LMS.Models.Enums;

namespace LMS.Entities
{
    public class Enrollment
    {
        public Guid Id { get; set; }
        public EnrollmentStatus Status { get; set; } = EnrollmentStatus.Active;
        public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public decimal PaidAmount { get; set; }

        // FK
        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;

        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public Guid? VoucherId { get; set; }
        public Voucher? Voucher { get; set; }
    }
}
