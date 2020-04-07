using BankAnalizer.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAnalizer.Db.Configurations
{
    public class BankTransactionGroupConfiguration : IEntityTypeConfiguration<BankTransactionGroup>
    {
        public void Configure(EntityTypeBuilder<BankTransactionGroup> builder)
        {
            builder.HasKey(bc => new { bc.BankTransactionId, bc.GroupId });

            builder.HasOne(bc => bc.BankTransaction)
                .WithMany(b => b.BankTransactionGroups)
                .HasForeignKey(bc => bc.BankTransactionId);

            builder.HasOne(bc => bc.Group)
                .WithMany(c => c.BankTransactionGroups)
                .HasForeignKey(bc => bc.GroupId);
        }
    }
}
