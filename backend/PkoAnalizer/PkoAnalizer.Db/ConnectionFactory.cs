using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PkoAnalizer.Db
{
    public class ConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(@"Server=192.168.99.104;Database=PkoAnalizer;Integrated Security=False;User Id=sa;Password=1Secure*Password1;MultipleActiveResultSets=true");
        }
    }
}
