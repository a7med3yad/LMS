using LMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configurations
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.TitleAr).IsRequired().HasMaxLength(300);
            builder.Property(e => e.TitleEn).IsRequired().HasMaxLength(300);

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Exam_PassScore",
                    "[PassScore] >= 0 AND [PassScore] <= 100"));

            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Exam_MaxAttempts",
                    "[MaxAttempts] >= 1"));

            // Exam → Questions
            builder.HasMany(e => e.Questions)
                .WithOne(q => q.Exam)
                .HasForeignKey(q => q.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            // Exam → Attempts
            builder.HasMany(e => e.Attempts)
                .WithOne(a => a.Exam)
                .HasForeignKey(a => a.ExamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
