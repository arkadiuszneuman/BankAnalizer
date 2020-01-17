using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PkoAnalizer.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PkoAnalizer.Db.Configurations
{
    public class RuleConfiguration : IEntityTypeConfiguration<Rule>
    {
        public void Configure(EntityTypeBuilder<Rule> builder)
        {
            builder.Property(p => p.RuleDefinition)
                .IsRequired();

            builder.Property(p => p.RuleName)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
