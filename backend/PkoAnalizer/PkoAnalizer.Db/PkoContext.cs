﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using PkoAnalizer.Db.Config;
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
        DbSet<Group> Groups { get; set; }
        DbSet<BankTransactionGroup> BankTransactionGroups { get; set; }

        Task LockTableAsync<T>(T table);
        ValueTask<EntityEntry> AddAsync([NotNull] object entity, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        EntityEntry Attach([NotNull] object entity);
        void AttachRange([NotNull] params object[] entities);
    }

    public class PkoContext : DbContext, IContext
    {
        private readonly ConnectionFactory connectionFactory;

        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<BankTransactionType> BankTransactionTypes { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<BankTransactionGroup> BankTransactionGroups { get; set; }

        public PkoContext()
        {
            this.connectionFactory = new ConnectionFactory(new SqlServerConfig
            {
                Database = "PkoAnalizer",
                Password = "1Secure*Password1",
                UserId = "sa",
                Server = "192.168.99.104"
            });
        }

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
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new BankTransactionGroupConfiguration());
        }
    }
}
