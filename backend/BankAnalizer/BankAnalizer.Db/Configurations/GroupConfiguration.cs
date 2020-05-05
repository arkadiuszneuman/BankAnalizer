using BankAnalizer.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace BankAnalizer.Db.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            builder.HasIndex(g => new { g.Name, g.UserId, g.RuleId }).IsUnique();
            builder.Property(g => g.Name).IsRequired();
        }
    }
}
