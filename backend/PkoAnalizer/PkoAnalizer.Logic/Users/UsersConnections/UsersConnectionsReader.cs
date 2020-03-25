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
        private readonly ConnectionFactory connectionFactory;

        public UsersConnectionsReader(ConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<UsersConnectionViewModel>> LoadUserConnections(Guid userId)
        {
            using var connection = connectionFactory.CreateConnection();

            return await connection.QueryAsync<UsersConnectionViewModel>(
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
                WHERE uc.UserRequestingConnectionId = @userId OR uc.UserRequestedToConnectId = @userId", new { userId });
        }
    }
}
