using Dapper;
using Microsoft.VisualBasic;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Persistence.Setting;

namespace YoutubeCloneBackend.Persistence.ValidateOtps
{
    public class ValidateOtp : IValidateOtp
    {
        private readonly string _connectionString;
        public ValidateOtp(ISetting setting)
        {
            _connectionString = setting.ConnectionString;
        }

        public async Task<OtpValidationModel?> VerifyRegisterationOtp(string email, string otp)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var cmdToValidateRegisterOtp = @"
                    SELECT 
                        message AS ""Message"",
                        is_verified AS ""IsSuccess"",
                        remaining_otp_attempt AS ""RemainingOtpAttempts"",
                        otp_expiry_time AS ""OtpExpiresAt"",
                        reattempt_after AS ""OtpReattemptAt""
                    FROM auth.fn_register_otp_validation(@p_email, @p_otp)";

                var parameters = new
                {
                    p_email = email,
                    p_otp = otp
                };

                // QuerySingleAsync will return only one row of data.
                var result = await connection.QuerySingleAsync<OtpValidationModel>(cmdToValidateRegisterOtp, parameters);
                if(result != null)
                {
                    if(result.OtpExpiresAt.HasValue)
                    {
                        result.OtpExpiresAt = result.OtpExpiresAt.Value.ToLocalTime();
                    }

                    if(result.OtpReattemptAt.HasValue)
                    {
                        result.OtpReattemptAt = result.OtpReattemptAt.Value.ToLocalTime();
                    }
                }

                return result;
            }
        }

        public async Task<OtpValidationModel?> VerifyResetPasswordOtp(string purpose, string email, string otp)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var cmdToValidateResetOtp = @"
                    SELECT 
                        is_success AS ""IsSuccess"",
                        message AS ""Message"",
                        otp_expiry_at AS ""OtpExpiresAt"",
                        remaining_otp_attempts AS ""RemainingOtpAttempts"",
                        reattempt_otp_at AS ""OtpReattemptAt""
                    FROM auth.fn_reset_otp_validation(@p_purpose, @p_email, @p_otp)";

                var parameter = new
                {
                    p_purpose = purpose,
                    p_email = email,
                    p_otp = otp
                };

                var result = await connection.QuerySingleAsync<OtpValidationModel>(cmdToValidateResetOtp, parameter);
                if(result != null )
                {
                    if(result.OtpExpiresAt.HasValue)
                    {
                        result.OtpExpiresAt = result.OtpExpiresAt.Value.ToLocalTime();
                    }

                    if(result.OtpReattemptAt.HasValue)
                    {
                        result.OtpReattemptAt = result.OtpReattemptAt.Value.ToLocalTime();
                    }
                }

                return result;
            }
        }
    }
}
