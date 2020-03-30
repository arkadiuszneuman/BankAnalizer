using System;

namespace PkoAnalizer.Logic.Users.UsersConnections.ViewModels
{
    public class UsersConnectionViewModel
    {
        public Guid RequestingUserId { get; set; }
        public string RequestingUserFirstName { get; set; }
        public string RequestingUserLastName { get; set; }
        public Guid RequestedUserId { get; set; }
        public string RequestedUserFirstName { get; set; }
        public string RequestedUserLastName { get; set; }
        public bool IsRequestApproved { get; set; }
    }
}
