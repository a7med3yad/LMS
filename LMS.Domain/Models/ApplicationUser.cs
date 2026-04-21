using LMS.Domain.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace LMS.Domain.Models
{

    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public UserRole Role { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        public string? EmailOtp { get; set; }
        public DateTime? OtpExpiration { get; set; }
        public int OtpRequestCount { get; set; }
        public DateTime? OtpRequestWindowStart { get; set; }



        public ICollection<Course> TaughtCourses { get; set; } = new List<Course>();
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();


    }
}
