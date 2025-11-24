using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;
using YoutubeCloneBackend.Core.User;

namespace YoutubeCloneBackend.Persistence.SendOtps
{
    public interface ISendOtp
    {
        Task<SentOtpModel?> InsertRegistrationOtpAsync(string email, string otp);
        Task<SentOtpModel?> InsertResetOtpAsync(string purpose, string email,  string otp);
    }
}
