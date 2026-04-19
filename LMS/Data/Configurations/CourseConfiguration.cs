using LMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.TitleAr).IsRequired().HasMaxLength(300);
            builder.Property(c => c.TitleEn).IsRequired().HasMaxLength(300);
            builder.Property(c => c.DescriptionAr).HasMaxLength(5000);
            builder.Property(c => c.DescriptionEn).HasMaxLength(5000);

            builder.Property(c => c.Price)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0);

            builder.Property(c => c.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Course → Materials (cascade delete: remove course = remove its materials)
            builder.HasMany(c => c.Materials)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Course → Assignments
            builder.HasMany(c => c.Assignments)
                .WithOne(a => a.Course)
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Course → Exams
            builder.HasMany(c => c.Exams)
                .WithOne(e => e.Course)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Course → Vouchers
            builder.HasMany(c => c.Vouchers)
                .WithOne(v => v.Course)
                .HasForeignKey(v => v.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index for common queries
            builder.HasIndex(c => c.Status);
            builder.HasIndex(c => c.InstructorId);
        }
    }
}
