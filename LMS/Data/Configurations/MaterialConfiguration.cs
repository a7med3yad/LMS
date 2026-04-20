using LMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configurations
{
    public class MaterialConfiguration : IEntityTypeConfiguration<Material>
    {
        public void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.TitleAr).IsRequired().HasMaxLength(300);
            builder.Property(m => m.TitleEn).IsRequired().HasMaxLength(300);
            builder.Property(m => m.DescriptionAr).HasMaxLength(2000);
            builder.Property(m => m.DescriptionEn).HasMaxLength(2000);
            builder.Property(m => m.ContentUrl).IsRequired();

            builder.Property(m => m.Type)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.HasIndex(m => m.CourseId);
            builder.HasIndex(m => new { m.CourseId, m.Order });
        }
    }
}
