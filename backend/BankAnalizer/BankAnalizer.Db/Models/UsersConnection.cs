using System;
using System.Collections.Generic;
using System.Text;

namespace BankAnalizer.Db.Models
{
    public class UsersConnection
    {
        public Guid Id { get; set; }
        public Guid UserRequestingConnectionId { get; set; }
        public User UserRequestingConnection { get; set; }
        public Guid UserRequestedToConnectId { get; set; }
        public User UserRequestedToConnect { get; set; }
        public bool IsRequestApproved { get; set; }
    }
}
