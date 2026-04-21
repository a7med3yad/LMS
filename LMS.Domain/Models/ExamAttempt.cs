
namespace LMS.Domain.Models
{
    public class ExamAttempt
    {
        public Guid Id { get; set; }
        public int Score { get; set; }
        public bool IsPassed { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SubmittedAt { get; set; }

        // FK
        public Guid ExamId { get; set; }
        public Exam Exam { get; set; } = null!;

        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;

        // Navigation
        public ICollection<ExamAnswer> Answers { get; set; } = [];
    }
}
