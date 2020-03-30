using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PkoAnalizer.Db.Models;

namespace PkoAnalizer.Db.Configurations
{
    public class UsersConnectionConfiguration : IEntityTypeConfiguration<UsersConnection>
    {
        public void Configure(EntityTypeBuilder<UsersConnection> builder)
        {
            builder.Property(s => s.UserRequestingConnectionId)
                .IsRequired();

            builder.Property(s => s.UserRequestedToConnectId)
                .IsRequired();

            builder.HasIndex(s => s.UserRequestingConnectionId);
            builder.HasIndex(s => s.UserRequestedToConnectId);

            builder.HasAlternateKey(s => new { s.UserRequestedToConnectId, s.UserRequestingConnectionId });
        }
    }
}
