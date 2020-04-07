using BankAnalizer.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankAnalizer.Db.Configurations
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
