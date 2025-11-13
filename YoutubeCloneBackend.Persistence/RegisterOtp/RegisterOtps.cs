using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YoutubeCloneBackend.Core.RegisterOtp;
using YoutubeCloneBackend.Persistence.Setting;

namespace YoutubeCloneBackend.Persistence.RegisterOtp
{
    public class RegisterOtps : IRegisterOtps
    {
        private readonly string _connectionString;
        public RegisterOtps(ISetting setting)
        {
            _connectionString = setting.ConnectionString;
        }

        public async Task<RegisterOtpResponseModel?> InsertRegistrationOtpAsync(string email, string otp)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var cmdToInsertOtp = "SELECT otp_expires_at AS \"OtpExpiresAt\" FROM public.fn_add_otp_to_temp_users(@p_email, @p_otp)";
                var parameter = new
                {
                    p_email = email,
                    p_otp = otp
                };

                var result = await connection.QueryFirstOrDefaultAsync<RegisterOtpResponseModel>(cmdToInsertOtp, parameter);
                if(result != null)
                {
                    result.OtpExpiresAt = result.OtpExpiresAt.ToLocalTime();
                }

                return result;
            }
        }
    }
}
