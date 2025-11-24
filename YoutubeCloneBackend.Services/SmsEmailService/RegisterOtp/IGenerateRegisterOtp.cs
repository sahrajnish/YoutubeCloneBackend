using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;
using YoutubeCloneBackend.Core.User;

namespace YoutubeCloneBackend.Services.SmsEmailService.RegisterOtp
{
    public interface IGenerateRegisterOtp
    {
        public Task<SentOtpModel> GenerateOtp(string email);
    }
}
