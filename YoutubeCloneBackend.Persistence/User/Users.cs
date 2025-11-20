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

        public async Task<GetUserResponse?> GetUser(string email)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                const string cmdCheckUser = "SELECT * FROM user_srvc.fn_get_user_from_users_table(@p_email)";

                var parameter = new
                {
                    p_email = email,
                };

                var result = await connection.QueryFirstOrDefaultAsync<GetUserResponse>(cmdCheckUser, parameter);
                return result;
            }
        }

        public async Task<InsertUserToTempTableResponse> InsertUserToTempTable(string email)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                const string cmdInsertUserToTempTable = "SELECT * FROM auth.fn_insert_user_to_temp_users_table(@p_email)";

                var parameter = new
                {
                    p_email = email,
                };

                var result = await connection.QueryFirstOrDefaultAsync<InsertUserToTempTableResponse>(cmdInsertUserToTempTable, parameter);
                return result;
            }
        }
    }
}
