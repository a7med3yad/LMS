using LMS.Models;
using LMS.Models.Enums;

namespace LMS.Entities
{
 
    public class Course
    {
        public Guid Id { get; set; }
        public string TitleAr { get; set; } = string.Empty;
        public string TitleEn { get; set; } = string.Empty;
        public string DescriptionAr { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public CourseStatus Status { get; set; } = CourseStatus.Draft;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public Guid InstructorId { get; set; }
        public ApplicationUser Instructor { get; set; } = null!;

        public ICollection<Material> Materials { get; set; } = new List<Material>();
        public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
        public ICollection<Exam> Exams { get; set; } = new List<Exam>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();

    }
}
