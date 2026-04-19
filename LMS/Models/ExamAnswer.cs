namespace LMS.Entities
{
    public class ExamAnswer
    {
        public Guid Id { get; set; }
        public string? OpenAnswer { get; set; }
        public int? ManualGrade { get; set; }

        // FK
        public Guid AttemptId { get; set; }
        public ExamAttempt Attempt { get; set; } = null!;

        public Guid QuestionId { get; set; }
        public Question Question { get; set; } = null!;

        public Guid? SelectedChoiceId { get; set; }
        public QuestionChoice? SelectedChoice { get; set; }
    }
}
