using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PkoAnalizer.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Db.Configurations
{
    public class BankTransactionTypeConfiguration : IEntityTypeConfiguration<BankTransactionType>
    {
        public void Configure(EntityTypeBuilder<BankTransactionType> builder)
        {
            builder.HasIndex(x => x.Name);
        }
    }
}
