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

        public async Task<UserResponse> InsertUser(UserSchema user)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                const string cmd = "SELECT * FROM public.fn_insert_user(" +
                    "@p_email, @p_first_name, @p_last_name, @p_password_hash, @p_mobile_number, " +
                    "@p_created_at, @p_is_mobile_verified, @p_is_email_verified, @p_account_status)";

                var status = user.AccountStatus;

                var parameter = new
                {
                    p_email = user.Email,
                    p_password_hash = user.PasswordHash,
                    p_first_name = user.FirstName,
                    p_last_name = user.LastName,
                    p_mobile_number = user.MobileNumber,
                    p_created_at = user.CreatedAt,
                    p_is_email_verified = user.IsEmailVerified,
                    p_is_mobile_verified = user.IsMobileVerified,
                    p_account_status = user.AccountStatus,
                };

                var result = await connection.QueryFirstOrDefaultAsync<UserResponse>(cmd, parameter);
                return result;
            }
        }
    }
}
