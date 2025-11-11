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

        public async Task<InsertUserToTempTableResponse> InsertUserService(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("Email is required");
            }

            if(!IsEmailValid(email))
            {
                throw new ArgumentException("Enter a valid email address");
            }

            var userInUserTable = await _user.GetUser(email);
            if (userInUserTable != null)
            {
                throw new ArgumentException("User already exist. Please login to continue.");
            }

            var userInTempTable = await _user.InsertUserToTempTable(email);
            return userInTempTable;
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
