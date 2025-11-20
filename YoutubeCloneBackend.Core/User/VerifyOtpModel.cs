using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCloneBackend.Core.User
{
    public class VerifyOtpDTOModel
    {
        [Required]
        public string Purpose { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Otp {  get; set; }
    }

    public class VerifyOtpResponseModel
    {
        public string Message { get; set; }
        public bool IsVerified { get; set; }
        public int? RemainingOtpAttempt { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public DateTime? ReattemptAfter { get; set; }
    }

    public class NewUserEvent
    {
        public string Email { get; set; }
    }
}
