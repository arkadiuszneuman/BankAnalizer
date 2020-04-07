using BankAnalizer.Db.Config;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BankAnalizer.Db
{
    public interface IConnectionFactory
    {
        IDbConnection CreateConnection();
        string CreateConnectionString();
    }

    public class ConnectionFactory : IConnectionFactory
    {
        private readonly SqlServerConfig sqlServerConfig;

        public ConnectionFactory(SqlServerConfig sqlServerConfig)
        {
            this.sqlServerConfig = sqlServerConfig;
        }

        public string CreateConnectionString()
        {
            var connectionStringBuilder = new SqlConnectionStringBuilder();
            connectionStringBuilder.UserID = sqlServerConfig.UserId;
            connectionStringBuilder.Password = sqlServerConfig.Password;
            connectionStringBuilder.InitialCatalog = sqlServerConfig.Database;
            connectionStringBuilder.IntegratedSecurity = false;
            connectionStringBuilder.MultipleActiveResultSets = true;
            connectionStringBuilder.DataSource = sqlServerConfig.Server;

            return connectionStringBuilder.ConnectionString;
        }

        public IDbConnection CreateConnection()
        {
            var connectionString = CreateConnectionString();
            return new SqlConnection(connectionString);
        }
    }
}
