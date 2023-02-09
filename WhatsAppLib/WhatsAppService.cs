using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using WhatsAppLib.Model;

namespace WhatsAppLib
{
    public class WhatsAppService : IWhatsAppInterface
    {
        private readonly WhatsAppDbConnection _dbContext;
        private bool _disposed;

        public WhatsAppService(WhatsAppDbConnection dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> CreateUser(UserViewModel user)
        {
            var sqlConnection = await _dbContext.OpenConnection();

            string query = $"INSERT INTO USERS ( Name, PhoneNumber, ProfilePicture)" +
                $"VALUES(@Name, @Phone, @Image) SELECT CAST(SCOPE_IDENTITY() AS BIGINT)";

            using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                SqlParameter parameter = new SqlParameter()
                {
                    ParameterName = "@Name",
                    Value = user.Name,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);

                parameter = new SqlParameter()
                {
                    ParameterName = "@Phone",
                    Value = user.PhoneNumber,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);

                parameter = new SqlParameter()
                {
                    ParameterName = "@Image",
                    Value = user.ProfilePicture,
                    SqlDbType = SqlDbType.NVarChar,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(parameter);

                //await command.ExecuteNonQueryAsync();

                long userId = (long)await command.ExecuteScalarAsync();

                return (int)userId;
            }


        }

        public async Task<int> DeleteUser(int UserId)
        {
            try
            {
                var sqlConnection = await _dbContext.OpenConnection();

                string query = $"DELETE From Users " +
                    $"Where UserId = @UserId";

                await using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    SqlParameter parameter = new SqlParameter()
                    {
                        ParameterName = "@UserId",
                        Value = UserId,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };

                    command.Parameters.Add(parameter);

                    await command.ExecuteScalarAsync();
                }


                return UserId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                return 0;
            }
        }

        public async Task<UserViewModel> GetUser(int UserId)
        {
            
                var sqlConnection = await _dbContext.OpenConnection();

                string query = $"SELECT Users.Name, Users.PhoneNumber, Users.ProfilePicture " +
                    $"From Users " +
                    $"Where UserId = @UserId";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    UserViewModel user = new UserViewModel();
                    SqlParameter parameter = new SqlParameter()
                    {
                        ParameterName = "@UserId",
                        Value = UserId,
                        SqlDbType = SqlDbType.NVarChar,
                        Direction = ParameterDirection.Input
                    };
                    command.Parameters.Add(parameter);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            user.Name = reader.GetString(0);
                            user.PhoneNumber = reader.GetString(1);
                            user.ProfilePicture = reader.GetString(2);

                            user.Name = reader["Name"].ToString();
                            user.PhoneNumber = reader["PhoneNumber"].ToString();
                            user.ProfilePicture = reader["ProfilePicture"].ToString();
                        }
                    }

                    Console.WriteLine($"Name: {user.Name} -- Phone Number: {user.PhoneNumber}");
                    Console.WriteLine(user.Name);
                    return user;
                };

               
            

        }

        public async Task<IEnumerable<UserViewModel>> GetUsers()
        {
             var sqlConnection = await _dbContext.OpenConnection();

            string query = "SELECT Users.Name, Users.PhoneNumber, Users.ProfilePicture " +
                "FROM Users";

            await using (SqlCommand command = new SqlCommand(query, sqlConnection))
            {
                List<UserViewModel> users = new List<UserViewModel>();

                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        users.Add(
                            new UserViewModel()
                            {
                                Name = reader["Name"].ToString(),
                                PhoneNumber = reader["PhoneNumber"].ToString(),
                                ProfilePicture = reader["ProfilePicture"].ToString(),
                            }   
                       );
                    }
                }

                foreach(var user in users)
                {
                    Console.WriteLine($"{user.Name} , {user.PhoneNumber}, {user.ProfilePicture}");
                }
                return users;
            }
        }

        public async Task<int> UpdateUser(int UserId, UserViewModel user)
        {
            try
            {

                var sqlconnection = await _dbContext.OpenConnection();



                string query =
                 $"UPDATE Users " +
                 $"SET Name = @Name, PhoneNumber = @Phone, ProfilePicture = @Image " +
                 $"WHERE UserID = @UserId ";



                using (SqlCommand command = new SqlCommand(query, sqlconnection))
                {
                    command.Parameters.AddRange(new SqlParameter[]
                    {
                        new SqlParameter
                        {
                            ParameterName = "@Name",
                            Value = user.Name,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Size = 50
                        },

                        new SqlParameter
                        {
                            ParameterName = "@Phone",
                            Value = user.PhoneNumber,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Size = 50
                        },
                        new SqlParameter
                        {
                            ParameterName = "@Image",
                            Value = user.ProfilePicture,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Size = 50
                        },

                        new SqlParameter
                        {
                            ParameterName = "@UserId",
                            Value = UserId,
                            SqlDbType = SqlDbType.NVarChar,
                            Direction = ParameterDirection.Input,
                            Size = 50
                        }

                    });


                    await command.ExecuteNonQueryAsync();
                    return UserId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine(ex.Source);
                return 0;
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            if (disposing)
            {
                _dbContext.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
