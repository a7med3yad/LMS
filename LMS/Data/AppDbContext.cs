using LMS.Entities;
using LMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions options)
       : base(options)
        {
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }



        public DbSet<Course> Courses { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionChoice> QuestionChoices { get; set; }
        public DbSet<ExamAttempt> ExamAttempts { get; set; }
        public DbSet<ExamAnswer> ExamAnswers { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }

}
