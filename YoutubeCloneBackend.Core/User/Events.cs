using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Core.User
{
    public class NotificationEvent
    {
        public NotificationPurpose Purpose { get; set; }
        public string Email { get; set; }
    }

    public class OtpEvent
    {
        public OtpPurpose Purpose { get; set; }
        public string Email { get; set; }
        public string Otp { get; set; }
    }

    public enum NotificationPurpose
    {
        WelcomeUser,
        PasswordCreated,
        PasswordReset,
        PasswordChanged
    }

    public enum OtpPurpose
    {
        Register,
        ResetPassword,
        Login
    }
}
