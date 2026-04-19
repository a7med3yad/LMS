using LMS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {

            builder.Property(u => u.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.AvatarUrl)
                .HasMaxLength(500);

            builder.Property(u => u.Role)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Instructor → Courses
            builder.HasMany(u => u.TaughtCourses)
                .WithOne(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Student → Enrollments
            builder.HasMany(u => u.Enrollments)
                .WithOne(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → Notifications
            builder.HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // User → RefreshTokens
            builder.HasMany(u => u.RefreshTokens)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
