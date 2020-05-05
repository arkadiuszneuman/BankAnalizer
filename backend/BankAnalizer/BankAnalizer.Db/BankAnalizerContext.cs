using BankAnalizer.Db.Config;
using BankAnalizer.Db.Configurations;
using BankAnalizer.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace BankAnalizer.Db
{
    public interface IContext : IDisposable, IAsyncDisposable
    {
        DbSet<BankTransaction> BankTransactions { get; set; }
        DbSet<BankTransactionType> BankTransactionTypes { get; set; }
        DbSet<Rule> Rules { get; set; }
        DbSet<Group> Groups { get; set; }
        DbSet<BankTransactionGroup> BankTransactionGroups { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UsersConnection> UsersConnections { get; set; }

        Task LockTableAsync<T>(T table);
        ValueTask<EntityEntry> AddAsync([NotNull] object entity, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry Attach([NotNull] object entity);
        void AttachRange([NotNull] params object[] entities);
    }

    public class BankAnalizerContext : DbContext, IContext
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<BankTransactionType> BankTransactionTypes { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<BankTransactionGroup> BankTransactionGroups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UsersConnection> UsersConnections { get; set; }

        public BankAnalizerContext(DbContextOptions<BankAnalizerContext> options) : base(options)
        {
        }

        public async Task LockTableAsync<T>(T table)
        {
            await Database.ExecuteSqlRawAsync($"SELECT TOP 0 NULL FROM {table.GetType().Name} WITH (TABLOCKX, HOLDLOCK)")
                .ConfigureAwait(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
                throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
