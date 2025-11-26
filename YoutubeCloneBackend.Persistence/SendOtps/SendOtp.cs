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

        public async Task<SentOtpModel?> InsertRegistrationOtpAsync(string email, string otp)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var cmdToInsertOtp = @"
                    SELECT
                        is_success AS ""IsSuccess"",
                        message AS ""Message"",
                        remaining_attempts AS ""RemainingResendAttempts"",
                        otp_expiry_at AS ""OtpExpiresAt"",
                        reattempt_resend_at AS ""ResendReattemptAt""
                    FROM auth.fn_add_otp_to_temp_users(@p_email, @p_otp)";         
                
                var parameter = new
                {
                    p_email = email,
                    p_otp = otp
                };

                // Update the Postgres database with new otp and also update the necessary fields.
                var result = await connection.QueryFirstOrDefaultAsync<SentOtpModel>(cmdToInsertOtp, parameter);
                if(result != null)
                {
                    if(result.OtpExpiresAt.HasValue)
                    {
                        // Convert the OTP Expiry time to Local time instead of GMT.
                        result.OtpExpiresAt = result.OtpExpiresAt.Value.ToLocalTime();
                    }
                    
                    if(result.ResendReattemptAt.HasValue)
                    {
                        // Convert the Reattempt time to Local time instead of GMT.
                        result.ResendReattemptAt = result.ResendReattemptAt.Value.ToLocalTime();
                    }
                }
                return result;
            }
        }
    }
}
