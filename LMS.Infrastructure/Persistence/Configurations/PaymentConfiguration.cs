using LMS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.HasKey(p => p.Id);

            // ── Decimal precision: prevents silent truncation ──
            builder.Property(p => p.Amount)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(p => p.Status)
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(p => p.Provider)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(p => p.ProviderTransactionId)
                .HasMaxLength(250);

            // ── Relationships ──

            // Payment → Student (no cascade: deleting a user should NOT auto-delete payments)
            builder.HasOne(p => p.Student)
                .WithMany()
                .HasForeignKey(p => p.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment → Course (no cascade: deleting a course should NOT auto-delete payments)
            builder.HasOne(p => p.Course)
                .WithMany()
                .HasForeignKey(p => p.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment → Enrollment (no cascade: prevent multiple cascade paths)
            builder.HasOne(p => p.Enrollment)
                .WithMany()
                .HasForeignKey(p => p.EnrollmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Indexes for common queries ──
            builder.HasIndex(p => p.StudentId);
            builder.HasIndex(p => p.CourseId);
            builder.HasIndex(p => p.Status);
            builder.HasIndex(p => p.CreatedAt);
        }
    }
}
