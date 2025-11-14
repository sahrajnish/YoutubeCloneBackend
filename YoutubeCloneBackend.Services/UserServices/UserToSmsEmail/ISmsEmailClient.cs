using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;

namespace YoutubeCloneBackend.Services.UserServices.UserToSmsEmail
{
    public interface ISmsEmailClient
    {
        public Task<RegisterOtpResponseModel?> SendOtpAsync(string email);
    }
}
