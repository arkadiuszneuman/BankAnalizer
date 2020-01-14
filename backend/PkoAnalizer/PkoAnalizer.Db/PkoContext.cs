using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Db.Configurations;
using PkoAnalizer.Db.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace PkoAnalizer.Db
{
    public interface IContext : IDisposable, IAsyncDisposable
    {
        DbSet<BankTransaction> BankTransactions { get; set; }
        DbSet<BankTransactionType> BankTransactionTypes { get; set; }
        DbSet<Rule> Rules { get; set; }

        Task LockTableAsync<T>(T table);
        ValueTask<EntityEntry> AddAsync([NotNull] object entity, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class PkoContext : DbContext, IContext
    {
        private readonly ConnectionFactory connectionFactory;

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<BankTransactionType> BankTransactionTypes { get; set; }
        public DbSet<Rule> Rules { get; set; }

        public PkoContext(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task LockTableAsync<T>(T table)
        {
            await Database.ExecuteSqlRawAsync($"SELECT TOP 0 NULL FROM {table.GetType().Name} WITH (TABLOCKX, HOLDLOCK)")
                .ConfigureAwait(true);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                //.UseLoggerFactory(MyLoggerFactory)
                .EnableSensitiveDataLogging()
                .UseSqlServer(connectionFactory.CreateConnectionString());

            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
                throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BankTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new BankTransactionTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RuleConfiguration());
        }
    }
}
