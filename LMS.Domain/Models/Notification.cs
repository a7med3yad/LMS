using LMS.Domain.Models.Enums;

namespace LMS.Domain.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string BodyAr { get; set; } = string.Empty;
        public string BodyEn { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
        public bool IsRead { get; set; } = false;
        public string? ActionUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // FK
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Guid? CourseId { get; set; }         // Optional: linked course
        public Guid? ReferenceId { get; set; }      // ID of the material/assignment/exam   
    }
}
