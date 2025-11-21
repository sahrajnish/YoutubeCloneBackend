using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Persistence.Setting;

namespace YoutubeCloneBackend.Persistence.SendOtps
{
    public class SendOtp : ISendOtp
    {
        private readonly string _connectionString;
        public SendOtp(ISetting setting)
        {
            _connectionString = setting.ConnectionString;
        }

        public async Task<RegisterOtpResponseModel?> InsertRegistrationOtpAsync(string email, string otp)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var cmdToInsertOtp = @"
                    SELECT
                        otp_expires_at AS ""OtpExpiresAt"",
                        remaining_attempts AS ""RemainingAttempts"",
                        reattempt_after AS ""ReattemptAfter""
                    FROM auth.fn_add_otp_to_temp_users(@p_email, @p_otp)";         
                
                var parameter = new
                {
                    p_email = email,
                    p_otp = otp
                };

                // Update the Postgres database with new otp and also update the necessary fields.
                var result = await connection.QueryFirstOrDefaultAsync<RegisterOtpResponseModel>(cmdToInsertOtp, parameter);
                if(result != null)
                {
                    if(result.OtpExpiresAt.HasValue)
                    {
                        // Convert the OTP Expiry time to Local time instead of GMT.
                        result.OtpExpiresAt = result.OtpExpiresAt.Value.ToLocalTime();
                    }
                    
                    if(result.ReattemptAfter.HasValue)
                    {
                        // Convert the Reattempt time to Local time instead of GMT.
                        result.ReattemptAfter = result.ReattemptAfter.Value.ToLocalTime();
                    }
                }
                return result;
            }
        }

        public async Task<SendResetOtpModel?> InsertResetOtpAsync(string purpose, string email, string otp)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var cmdToInsertResetOtp = @"
                    SELECT
                        is_success AS ""IsSuccess"",
                        message AS ""Message"",
                        otp_expiry_at AS ""OtpExpiresAt"",
                        resend_reattempt_time AS ""ResendReattemptAt"",
                        remaining_resend_attempts AS ""RemainingResendAttempts"" 
                    FROM auth.fn_send_reset_otp(@p_purpose, @p_email, @p_otp)";

                var parameter = new
                {
                    p_purpose = purpose,
                    p_email = email,
                    p_otp = otp
                };

                var result = await connection.QueryFirstOrDefaultAsync<SendResetOtpModel>(cmdToInsertResetOtp, parameter);

                if( result != null )
                {
                    if(result.OtpExpiresAt.HasValue)
                    {
                        result.OtpExpiresAt = result.OtpExpiresAt.Value.ToLocalTime();
                    }
                    if(result.ResendReattemptAt.HasValue)
                    {
                        result.ResendReattemptAt = result.ResendReattemptAt.Value.ToLocalTime();
                    }
                }

                return result;
            }
        }
    }
}
