using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;
using YoutubeCloneBackend.Core.User;

namespace YoutubeCloneBackend.Services.UserServices.User
{
    public interface IUserService
    {
        // Insert new user to temp table.
        Task<SentOtpModel> InsertUserToTempTableService(string email);
        // Create Password based on Purpose - Purpose = "NewPassword", Purpose = "ResetPassword", Purpose = "Unlock"
        Task<CreatePasswordResponseModel?> CreateNewPasswordService(NewPasswordTypes purpose, string email, string plainPassword, string confirmPassword);
        // Send Reset Otp to User.
        Task<SentOtpModel?> SendResetOtpService(string email);
        Task<SentOtpModel?> ResendOtpService(string email);
        Task<OtpValidationModel?> VerifyOtpService(OtpPurpose Purpose, string Email, string Otp);
    }
}
