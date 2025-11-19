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
        Task<VerifyOtpResponseModel?> VerifyRegisterationOtp(string email, string otp);
    }
}
