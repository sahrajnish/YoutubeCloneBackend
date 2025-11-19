using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;

namespace YoutubeCloneBackend.Services.UserServices.OtpValidation
{
    public interface IOtpValidations
    {
        Task<VerifyOtpResponseModel?> VerifyRegisterOtpService(string Purpose, string Email, string Otp);
    }
}
