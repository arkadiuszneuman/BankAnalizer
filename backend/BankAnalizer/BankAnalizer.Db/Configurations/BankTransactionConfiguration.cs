using BankAnalizer.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAnalizer.Db.Configurations
{
    public class BankTransactionConfiguration : IEntityTypeConfiguration<BankTransaction>
    {
        public void Configure(EntityTypeBuilder<BankTransaction> builder)
        {
            builder.Property(p => p.Currency)
                .IsRequired();
            
            builder.Property(x => x.OperationDate)
                .HasColumnType("Date");

            builder.Property(x => x.TransactionDate)
                .HasColumnType("Date");
        }
    }
}
