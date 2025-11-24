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
        Task<SentOtpModel> InsertUserToTempTableService(string email);
        Task<CreatePasswordResponseModel?> CreateNewPasswordService(string email, string plainPassword, string confirmPassword);
        Task<SentOtpModel?> SendResetOtpService(string email);
    }
}
