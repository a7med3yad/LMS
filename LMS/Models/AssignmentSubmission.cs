using LMS.Models;

namespace LMS.Entities
{
    public class AssignmentSubmission
    {
        public Guid Id { get; set; }
        public string? TextAnswer { get; set; }
        public string? FileUrl { get; set; }
        public int Grade { get; set; }
        public string? Feedback { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public DateTime? GradedAt { get; set; } 

        public Guid AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;

        public Guid StudentId { get; set; }
        public ApplicationUser Student { get; set; } = null!;
    }
}
