namespace LMS.Entities
{
    public class Exam
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public int DurationMinutes { get; set; }
        public int PassScore { get; set; }        
        public int MaxAttempts { get; set; } = 1;
        public bool IsPublished { get; set; } = false;
        public DateTime? AvailableFrom { get; set; }
        public DateTime? AvailableUntil { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        // Navigation
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<ExamAttempt> Attempts { get; set; } = new List<ExamAttempt>();
    }
}
