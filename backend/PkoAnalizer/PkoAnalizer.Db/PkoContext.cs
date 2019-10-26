using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using PkoAnalizer.Db.Configurations;
using PkoAnalizer.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PkoAnalizer.Db
{
    public class PkoContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public DbSet<BankTransaction> BankTransactions { get; set; }
        public DbSet<BankTransactionType> BankTransactionTypes { get; set; }

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
                .UseSqlServer(@"Server=192.168.99.104;Database=PkoAnalizer;Integrated Security=False;User Id=sa;Password=1Secure*Password1;MultipleActiveResultSets=true");

            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
                throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BankTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new BankTransactionTypeConfiguration());
        }
    }
}
