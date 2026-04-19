using LMS.Models.Enums;

namespace LMS.Entities
{
    public class Question
    {
        public Guid Id { get; set; }
        public string TextAr { get; set; } = string.Empty;
        public string TextEn { get; set; } = string.Empty;
        public QuestionType Type { get; set; }
        public int Points { get; set; } = 1;
        public int Order { get; set; }

        public Guid ExamId { get; set; }
        public Exam Exam { get; set; } = null!;

        public ICollection<QuestionChoice> Choices { get; set; } = [];
    }
}
