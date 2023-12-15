using Microsoft.Data.SqlClient;

namespace WebKursovaya.ViewModels
{
    public class SqlLoginsService
    {
        private readonly string _connectionString;

        public SqlLoginsService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public bool ValidateLogin(string username, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = $"SELECT COUNT(*) FROM sys.sql_logins WHERE name = @username AND password_hash = HASHBYTES('SHA2_512', @password_hash);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password_hash", password);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
    }
}
