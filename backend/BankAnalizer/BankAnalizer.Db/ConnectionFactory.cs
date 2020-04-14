using BankAnalizer.Db.Config;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Data;

namespace BankAnalizer.Db
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class ConnectionFactory : IConnectionFactory
    {
        private readonly DbContextOptions<BankAnalizerContext> options;

        public ConnectionFactory(DbContextOptions<BankAnalizerContext> options)
        {
            this.options = options;
        }

        public IDbConnection CreateConnection()
        {
            var extension = options.FindExtension<SqlServerOptionsExtension>();
            return new SqlConnection(extension.ConnectionString);
        }
    }
}
