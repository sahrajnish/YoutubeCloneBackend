using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;

namespace YoutubeCloneBackend.Services.SmsEmailService.RegisterOtp
{
    public interface IGenerateRegisterOtp
    {
        public Task<RegisterOtpResponseModel> GenerateOtp(string email);
    }
}
