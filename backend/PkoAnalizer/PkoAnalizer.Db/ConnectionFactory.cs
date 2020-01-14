using Microsoft.Data.SqlClient;
using PkoAnalizer.Db.Config;
using System.Data;

namespace PkoAnalizer.Db
{
    public class ConnectionFactory
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
