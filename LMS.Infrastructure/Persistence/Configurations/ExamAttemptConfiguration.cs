using LMS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations
{
    public class ExamAttemptConfiguration : IEntityTypeConfiguration<ExamAttempt>
    {
        public void Configure(EntityTypeBuilder<ExamAttempt> builder)
        {
            builder.HasKey(a => a.Id);

            // Index for looking up attempts by student + exam
            builder.HasIndex(a => new { a.StudentId, a.ExamId });

            builder.HasMany(a => a.Answers)
                .WithOne(ans => ans.Attempt)
                .HasForeignKey(ans => ans.AttemptId)
                .OnDelete(DeleteBehavior.Cascade);

            // Student attempts: no cascade on student delete (keep exam history)
            builder.HasOne(a => a.Student)
                .WithMany()
                .HasForeignKey(a => a.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
