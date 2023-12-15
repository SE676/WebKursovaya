using Microsoft.Data.SqlClient;

namespace WebKursovaya.Models
{
    public class DecryptDataService
    {
        private readonly IConfiguration _configuration; // Ваша зависимость IConfiguration

        public DecryptDataService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string DecryptData(string encryptedData)
        {
            string decryptedData = string.Empty;
            string connectionString = _configuration.GetConnectionString("ThirdConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT CONVERT(VARCHAR(MAX), DECRYPTBYPASSPHRASE('el', @encryptedData)) AS DecryptedData", connection);
                command.Parameters.AddWithValue("@encryptedData", encryptedData);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    decryptedData = reader["DecryptedData"].ToString();
                }
            }

            return decryptedData;
        }

    }
}
