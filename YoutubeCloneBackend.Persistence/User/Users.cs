using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.User;
using YoutubeCloneBackend.Persistence.Setting;

namespace YoutubeCloneBackend.Persistence.User
{
    public class Users : IUsers
    {
        private readonly string _connectionString;
        public Users(ISetting setting)
        {
            _connectionString = setting.ConnectionString;
        }

        public async Task<CreatePasswordResponseModel?> CreateNewPassword(string email, string passwordHash)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                const string cmdToCreatePassword = @"
                    SELECT 
                        is_success AS ""IsSuccess"",
                        message AS ""Message""
                    FROM user_srvc.fn_create_new_password(@p_email, @p_password_hash)";

                var parameter = new
                {
                    p_email = email,
                    p_password_hash = passwordHash
                };

                var result = await connection.QueryFirstOrDefaultAsync<CreatePasswordResponseModel>(cmdToCreatePassword, parameter);

                return result;
            }
        }

        public async Task<InsertUserToTempTableResponse?> InsertUserToTempTable(string email)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                const string cmdInsertUserToTempTable = @"
                    SELECT 
                        is_success AS ""IsSuccess"",
                        message AS ""Message"",
                        user_id AS ""Id""
                    FROM auth.fn_insert_user_to_temp_users_table(@p_email)";

                var parameter = new
                {
                    p_email = email,
                };

                var result = await connection.QueryFirstOrDefaultAsync<InsertUserToTempTableResponse>(cmdInsertUserToTempTable, parameter);
                return result;
            }
        }

        public async Task<CreatePasswordResponseModel?> ResetPassword(string purpose, string email, string passwordHash)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                const string cmtToResetPassword = @"
                    SELECT 
                        is_success AS ""IsSuccess"",
                        message AS ""Message"" 
                    FROM user_srvc.fn_reset_password(@p_purpose, @p_email, @p_new_password_hash)";

                var parameter = new
                {
                    p_purpose = purpose,
                    p_email = email,
                    p_new_password_hash = passwordHash
                };

                var result = await connection.QueryFirstOrDefaultAsync<CreatePasswordResponseModel>(cmtToResetPassword, parameter);
                return result;
            }
        }

        public async Task<SentOtpModel?> InsertResetOtpAsync(string purpose, string email, string otp)
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

                var result = await connection.QueryFirstOrDefaultAsync<SentOtpModel>(cmdToInsertResetOtp, parameter);

                if (result != null)
                {
                    if (result.OtpExpiresAt.HasValue)
                    {
                        result.OtpExpiresAt = result.OtpExpiresAt.Value.ToLocalTime();
                    }
                    if (result.ResendReattemptAt.HasValue)
                    {
                        result.ResendReattemptAt = result.ResendReattemptAt.Value.ToLocalTime();
                    }
                }

                return result;
            }
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
                if (result != null)
                {
                    if (result.OtpExpiresAt.HasValue)
                    {
                        result.OtpExpiresAt = result.OtpExpiresAt.Value.ToLocalTime();
                    }

                    if (result.OtpReattemptAt.HasValue)
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
                if (result != null)
                {
                    if (result.OtpExpiresAt.HasValue)
                    {
                        result.OtpExpiresAt = result.OtpExpiresAt.Value.ToLocalTime();
                    }

                    if (result.OtpReattemptAt.HasValue)
                    {
                        result.OtpReattemptAt = result.OtpReattemptAt.Value.ToLocalTime();
                    }
                }

                return result;
            }
        }
    }
}
