using Dapper;
using PkoAnalizer.Db;
using PkoAnalizer.Logic.Users.UsersConnections.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PkoAnalizer.Logic.Users.UsersConnections
{
    public class UsersConnectionsReader
    {
        public class UsersConnectionsFilter
        {
            public bool ShowAlsoAsRequestedUser { get; set; }
            public bool OnlyApproved { get; set; }
        }

        private readonly IConnectionFactory connectionFactory;

        public UsersConnectionsReader(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<UsersConnectionViewModel>> LoadUserConnections(UsersConnectionsFilter filter, Guid userId)
        {
            using var connection = connectionFactory.CreateConnection();

            var builder = new SqlBuilder();
            var selector = builder.AddTemplate(
                @"SELECT 
                  requestingConnectionUser.Id as RequestingUserId,
                  requestingConnectionUser.FirstName as RequestingUserFirstName,
                  requestingConnectionUser.LastName as RequestingUserLastName,
                  userRequestedDoConnect.Id as RequestedUserId,
                  userRequestedDoConnect.FirstName as RequestedUserFirstName,
                  userRequestedDoConnect.LastName as RequestedUserLastName,
                  uc.IsRequestApproved as IsRequestApproved
                FROM UsersConnections AS uc
                JOIN Users requestingConnectionUser ON requestingConnectionUser.Id = uc.UserRequestingConnectionId
                JOIN Users userRequestedDoConnect ON userRequestedDoConnect.Id = uc.UserRequestedToConnectId
                /**where**/");

            if (filter.OnlyApproved)
                builder.Where("uc.IsRequestApproved = 1", new { userId });

            builder.OrWhere("uc.UserRequestingConnectionId = @userId", new { userId });

            if (filter.ShowAlsoAsRequestedUser)
                builder.OrWhere("uc.UserRequestedToConnectId = @userId", new { userId });

            return await connection.QueryAsync<UsersConnectionViewModel>(selector.RawSql, selector.Parameters);
        }
    }
}
