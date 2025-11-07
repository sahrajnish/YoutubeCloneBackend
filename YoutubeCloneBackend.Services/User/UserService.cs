using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                if(!IsEmailValid(user.Email))
                {
                    throw new ArgumentException("Not a valid email");
                }
                userDetails.Email = user.Email.Trim().ToLower();
            }

            if (!string.IsNullOrEmpty(user.Password))
            {
                if(!IsPasswordValid(user.Password))
                {
                    throw new ArgumentException("Password must include minimum 8 characters, 1 upper, 1 lower, 1 number, 1 special");
                }
                userDetails.PasswordHash = HashPassword(user.Password);
            }

            if (!string.IsNullOrEmpty(user.FirstName))
            {
                userDetails.FirstName = user.FirstName.Trim();
            }

            if (!string.IsNullOrEmpty(user.LastName))
            {
                userDetails.LastName = user.LastName.Trim();
            }

            if (!string.IsNullOrEmpty(user.MobileNumber))
            {
                if(!IsMobileValid(user.MobileNumber))
                {
                    throw new ArgumentException("Enter a valid Mobile Number");
                }
                userDetails.MobileNumber = user.MobileNumber.Trim();
            }

            var result = await _user.InsertUser(userDetails);
            return result;
        }   

        private string HashPassword(string PlainTextPassword)
        {
            string hash = BCrypt.Net.BCrypt.HashPassword(PlainTextPassword);
            return hash;
        }

        private bool IsEmailValid(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }

        private bool IsPasswordValid(string password)
        {
            if(string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            // Regex: Minimum 8 chars, 1 upper, 1 lower, 1 number, 1 special
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");
            return regex.IsMatch(password);
        }

        private bool IsMobileValid(string mobile)
        {
            if(string.IsNullOrWhiteSpace(mobile))
            {
                return false;
            }

            // Indian mobile: starts 6-9, 10 digits
            var regex = new Regex(@"^[6-9]\d{9}$");
            return regex.IsMatch(mobile.Trim());
        }
    }
}
