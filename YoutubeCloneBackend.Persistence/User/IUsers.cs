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
        Task<InsertUserToTempTableResponse?> InsertUserToTempTable(string email);
        Task<CreatePasswordResponseModel?> CreateNewPassword(string email, string passwordHash);
        Task<CreatePasswordResponseModel?> ResetPassword(string purpose, string email, string passwordHash);
        Task<SentOtpModel?> InsertResetOtpAsync(string purpose, string email, string otp);
        Task<OtpValidationModel?> VerifyRegisterationOtp(string email, string otp);
        Task<OtpValidationModel?> VerifyResetPasswordOtp(string purpose, string email, string otp);
    }
}
