using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebKursovaya.Models;

namespace WebKursovaya.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SqlLoginsController : Controller
    {
        private readonly IConfiguration _configuration;

        public SqlLoginsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetSqlLogins()
        {
            string connectionString = _configuration.GetConnectionString("SecondConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT name, password_hash FROM master.sys.sql_logins"; // Предположим, что столбцы называются name и password_hash

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);

                    // Преобразование DataTable в список объектов с выбранными полями
                    List<sql_logins> loginsList = ConvertDataTableToList(dt);

                    return View("Index", loginsList); 
                }
            }
        }
        private List<sql_logins> ConvertDataTableToList(DataTable dataTable)
        {
            var dataList = new List<sql_logins>();

            foreach (DataRow row in dataTable.Rows)
            {
                var login = new sql_logins
                {
                    name = row["name"].ToString(),
                    password_hash = ConvertByteArrayToString((byte[])row["password_hash"])
                };
                dataList.Add(login);
            }
            return dataList;
        }

        private string ConvertByteArrayToString(byte[] byteArray)
        {
            return "0x" + BitConverter.ToString(byteArray).Replace("-", "");
        }

    }
}

