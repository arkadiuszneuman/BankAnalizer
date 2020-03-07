using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PkoAnalizer.Db.Models;

namespace PkoAnalizer.Db.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.FirstName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.LastName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(p => p.Username)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasAlternateKey(p => p.Username);

            builder.Property(p => p.PasswordHash)
                .IsRequired();

            builder.Property(p => p.PasswordSalt)
                .IsRequired();
        }
    }
}
