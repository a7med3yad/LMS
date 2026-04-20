using LMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.Id);

            builder.Property(n => n.TitleAr).IsRequired().HasMaxLength(300);
            builder.Property(n => n.TitleEn).IsRequired().HasMaxLength(300);
            builder.Property(n => n.BodyAr).IsRequired().HasMaxLength(2000);
            builder.Property(n => n.BodyEn).IsRequired().HasMaxLength(2000);

            builder.Property(n => n.Type)
                .HasConversion<string>()
                .HasMaxLength(30);

            builder.HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(n => n.UserId);
            builder.HasIndex(n => new { n.UserId, n.IsRead });
        }
    }
}
