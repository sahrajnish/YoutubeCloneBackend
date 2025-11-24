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
        public string Email { get; set; }
        [Required]
        public string Otp {  get; set; }
    }

    public class OtpDTOModel
    {
        [Required]
        public string Email { get; set; }
    }

    public class SentOtpModel
    {
        public bool IsSuccess {  get; set; }
        public string Message { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public DateTime? ResendReattemptAt { get; set; }
        public int? RemainingResendAttempts { get; set; }
    }

    public class OtpValidationModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public DateTime? OtpExpiresAt { get; set; }
        public DateTime? OtpReattemptAt { get; set; }
        public int? RemainingOtpAttempts { get; set; }
    }

    public class NewUserEvent
    {
        public string Email { get; set; }
    }

    public enum ResetPurpose
    {
        ResetPassword,
        Unlock
    }
}
