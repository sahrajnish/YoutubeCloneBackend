using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Core.User
{
    public class GetUserResponse
    {
        public int Guid { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class InsertUserToTempTableResponse
    {
        public int Guid { get; set; }
        public string Email { get; set; }
        public DateTime EmailAddedAt { get; set; }
        public DateTime EmailExpiresAt { get; set; }
    }

    public class UserDTO
    {
        public required string Email { get; set; }
    }

    //public enum AccountStatusType
    //{
    //    Active,
    //    Inactive,
    //    Blocked,
    //    Pending,
    //    Locked,
    //    Deleted
    //}
}
