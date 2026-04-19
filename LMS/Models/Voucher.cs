namespace LMS.Entities
{
    public class Voucher
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;          // Unique code
        public decimal DiscountPercent { get; set; }               // 0–100
        public decimal? DiscountAmount { get; set; }               // Fixed amount alternative
        public int MaxUses { get; set; } = 1;
        public int UsedCount { get; set; } = 0;
        public DateTime ExpiresAt { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        // Navigation
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();

    }
}
