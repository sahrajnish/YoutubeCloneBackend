using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Core.User
{
    public class UserSchema
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string MobileNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsMobileVerified { get; set; } = false;
        public bool IsEmailVerified { get; set; } = false;
        public string AccountStatus { get; set; } = AccountStatusType.Pending.ToString();
    }

    public class UserResponse
    {
        public int Id { get; set; }
        public string Message { get; set; }
    }

    public class UserSchemaDTO
    {
        public required string Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MobileNumber { get; set; }
    }

    public enum AccountStatusType
    {
        Active,
        Inactive,
        Blocked,
        Pending,
        Locked,
        Deleted
    }
}
