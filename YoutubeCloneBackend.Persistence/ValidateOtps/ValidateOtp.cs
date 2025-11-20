using Dapper;
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

        public async Task<VerifyOtpResponseModel?> VerifyRegisterationOtp(string email, string otp)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var cmdToValidateRegisterOtp = @"
                    SELECT 
                        message AS ""Message"",
                        is_verified AS ""IsVerified"",
                        remaining_otp_attempt AS ""RemainingOtpAttempt"",
                        otp_expiry_time AS ""OtpExpiresAt"",
                        reattempt_after AS ""ReattemptAfter""
                    FROM auth.fn_register_otp_validation(@p_email, @p_otp)";

                var parameters = new
                {
                    p_email = email,
                    p_otp = otp
                };

                // QuerySingleAsync will return only one row of data.
                var result = await connection.QuerySingleAsync<VerifyOtpResponseModel>(cmdToValidateRegisterOtp, parameters);
                if(result != null)
                {
                    if(result.OtpExpiresAt.HasValue)
                    {
                        result.OtpExpiresAt = result.OtpExpiresAt.Value.ToLocalTime();
                    }

                    if(result.ReattemptAfter.HasValue)
                    {
                        result.ReattemptAfter = result.ReattemptAfter.Value.ToLocalTime();
                    }
                }

                return result;
            }
        }
    }
}
