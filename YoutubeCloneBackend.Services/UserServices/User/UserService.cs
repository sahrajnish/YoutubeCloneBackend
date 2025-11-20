using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Messaging.Services;
using YoutubeCloneBackend.Persistence.User;
using YoutubeCloneBackend.Services.UserServices.UserToSmsEmail;

namespace YoutubeCloneBackend.Services.UserServices.User
{
    public class UserService : IUserService
    {
        private readonly IUsers _user;
        private readonly ISmsEmailClient _smsEmailClient;
        public UserService(IUsers user, ISmsEmailClient smsEmailClient)
        {
            _user = user;
            _smsEmailClient = smsEmailClient;
        }

        public async Task<RegisterOtpResponseModel> InsertUserToTempTableService(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException("Email is required");
            }

            if(!IsEmailValid(email))
            {
                throw new ArgumentException("Enter a valid email address");
            }

            // Check if user already exists in User Table.
            var userInUserTable = await _user.GetUser(email);
            if (userInUserTable != null)
            {
                throw new ArgumentException("User already exist. Please login to continue.");
            }

            // If User does not exist in user table then insert user to temp table
            var userInTempTable = await _user.InsertUserToTempTable(email);
            if(userInTempTable == null)
            {
                throw new Exception("Something went wrong while creating temp user.");
            }

            // Call SmsEmilService API to generate OTP and send it to user's email.
            var otpData = await _smsEmailClient.SendOtpAsync(email);
            if(otpData == null)
            {
                throw new Exception("Failed to generate OTP. Please try again later.");
            }

            return otpData;
        }

        public async Task<CreatePasswordResponseModel> CreateNewPasswordService(string email, string plainPassword, string confirmPassword)
        {
            if(string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required", nameof(email));
            }

            if(string.IsNullOrWhiteSpace(plainPassword))
            {
                throw new ArgumentException("Password is required", nameof(plainPassword));
            }

            if(plainPassword != confirmPassword)
            {
                throw new ArgumentException("Both Passwords do not match.");
            }
            
            if(!IsPasswordValid(plainPassword)) 
            {
                throw new ArgumentException("Password must contain 8 characters including 1 uppercase, 1 lowercase, 1 digit, 1 special character");
            }

            var passwordHash = HashPassword(plainPassword);

            var result = await _user.CreateNewPassword(email, passwordHash);
            if(result == null)
            {
                throw new InvalidOperationException("Database did not return any result.");
            }

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
