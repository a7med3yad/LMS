using LMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configurations
{
    public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
    {
        public void Configure(EntityTypeBuilder<Enrollment> builder)
        {
            builder.HasKey(e => e.Id);

            // A student can only enroll ONCE in a course
            builder.HasIndex(e => new { e.StudentId, e.CourseId })
                .IsUnique();

            builder.Property(e => e.PaidAmount)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(e => e.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Voucher is optional
            builder.HasOne(e => e.Voucher)
                .WithMany(v => v.Enrollments)
                .HasForeignKey(e => e.VoucherId)
                .OnDelete(DeleteBehavior.SetNull)
                .IsRequired(false);

            // Soft constraint: don't cascade delete course (restrict to prevent data loss)
            builder.HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
