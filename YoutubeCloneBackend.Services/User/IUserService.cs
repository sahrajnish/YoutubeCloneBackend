using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;

namespace YoutubeCloneBackend.Services.User
{
    public interface IUserService
    {
        Task<UserResponse> InsertUserService(UserSchemaDTO user);
    }
}
