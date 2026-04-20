using LMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.TitleAr).IsRequired().HasMaxLength(300);
            builder.Property(a => a.TitleEn).IsRequired().HasMaxLength(300);
            builder.Property(a => a.DescriptionAr).IsRequired();
            builder.Property(a => a.DescriptionEn).IsRequired();

            builder.Property(a => a.SubmissionType)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Assignment → Submissions
            builder.HasMany(a => a.Submissions)
                .WithOne(s => s.Assignment)
                .HasForeignKey(s => s.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(a => a.CourseId);
        }
    }
}
