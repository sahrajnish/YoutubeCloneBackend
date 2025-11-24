using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;

namespace YoutubeCloneBackend.Persistence.ValidateOtps
{
    public interface IValidateOtp
    {
        Task<OtpValidationModel?> VerifyRegisterationOtp(string email, string otp);

        Task<OtpValidationModel?> VerifyResetPasswordOtp(string purpose, string email, string otp);
    }
}
