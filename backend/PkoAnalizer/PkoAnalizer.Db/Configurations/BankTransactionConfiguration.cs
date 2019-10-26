using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PkoAnalizer.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Db.Configurations
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
