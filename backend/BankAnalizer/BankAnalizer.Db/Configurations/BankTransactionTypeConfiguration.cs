using BankAnalizer.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAnalizer.Db.Configurations
{
    public class BankTransactionTypeConfiguration : IEntityTypeConfiguration<BankTransactionType>
    {
        public void Configure(EntityTypeBuilder<BankTransactionType> builder)
        {
            builder.HasIndex(x => x.Name);
        }
    }
}
