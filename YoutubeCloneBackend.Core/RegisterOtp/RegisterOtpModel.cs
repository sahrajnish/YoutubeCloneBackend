using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Core.RegisterOtp
{
    public class RegisterOtpResponseModel
    {
        public DateTime? OtpExpiresAt { get; set; }
        public int RemainingAttempts { get; set; }
        public DateTime? ReattemptAfter { get; set; }
    }
}
