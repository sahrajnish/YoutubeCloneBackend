using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Persistence.User;

namespace YoutubeCloneBackend.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUsers _user;
        public UserService(IUsers user)
        {
            _user = user;
        }

        public async Task<UserResponse> InsertUserService(UserSchemaDTO user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("User details are required");
            }

            UserSchema userDetails = new UserSchema();

            if (string.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("Email is required.");
            }
            else
            {
                userDetails.Email = user.Email;
            }

            if (string.IsNullOrEmpty(user.Password))
            {

            }

            if (!string.IsNullOrEmpty(user.FirstName))
            {
                userDetails.FirstName = user.FirstName;
            }

            if (!string.IsNullOrEmpty(user.LastName))
            {
                userDetails.LastName = user.LastName;
            }

            if (!string.IsNullOrEmpty(user.MobileNumber))
            {
                userDetails.MobileNumber = user.MobileNumber;
            }

            var result = await _user.InsertUser(userDetails);
            return result;
        }
    }
}
