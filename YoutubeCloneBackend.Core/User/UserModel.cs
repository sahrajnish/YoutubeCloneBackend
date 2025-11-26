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
        public Guid Id { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
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
