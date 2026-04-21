namespace LMS.Domain.Models
{
    public class QuestionChoice
    {
        public Guid Id { get; set; }
        public string TextAr { get; set; } = string.Empty;
        public string TextEn { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        // FK
        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = null!;
    }
}
