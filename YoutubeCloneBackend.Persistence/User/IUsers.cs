using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;

namespace YoutubeCloneBackend.Persistence.User
{
    public interface IUsers
    {
        Task<GetUserResponse?> GetUser(string email);
        Task<InsertUserToTempTableResponse> InsertUserToTempTable(string email);
        Task<CreatePasswordResponseModel?> CreateNewPassword(string email, string passwordHash);
        Task<CreatePasswordResponseModel?> ResetPassword(string purpose, string email, string passwordHash);
    }
}
