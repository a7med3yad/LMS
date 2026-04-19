using LMS.Models.Enums;

namespace LMS.Entities
{
  
    public class Assignment
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public SubmissionType SubmissionType { get; set; }
        public DateTime DeadLine { get; set; }
        public int MaxGrade { get; set; } = 100;
        public bool IsPublished { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();

    }
}
