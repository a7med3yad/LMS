using LMS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LMS.Data.Configurations
{
    public class VoucherConfiguration : IEntityTypeConfiguration<Voucher>
    {
        public void Configure(EntityTypeBuilder<Voucher> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Code)
                .IsRequired()
                .HasMaxLength(50);

            // Code must be unique globally
            builder.HasIndex(v => v.Code).IsUnique();

            builder.Property(v => v.DiscountPercent)
                .HasColumnType("decimal(5,2)");

            builder.Property(v => v.DiscountAmount)
                .HasColumnType("decimal(18,2)");

            // Check constraint: UsedCount cannot exceed MaxUses
            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Voucher_UsedCount",
                    "[UsedCount] <= [MaxUses]"));

            // Check constraint: DiscountPercent between 0 and 100
            builder.ToTable(t =>
                t.HasCheckConstraint("CK_Voucher_DiscountPercent",
                    "[DiscountPercent] >= 0 AND [DiscountPercent] <= 100"));
        }
    }
}
