using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;

namespace YoutubeCloneBackend.Persistence.RegisterOtp
{
    public interface IRegisterOtps
    {
        Task<RegisterOtpResponseModel?> InsertRegistrationOtpAsync(string email, string otp);
    }
}
