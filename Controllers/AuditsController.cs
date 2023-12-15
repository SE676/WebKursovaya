using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using WebKursovaya.Models;

namespace WebKursovaya.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuditsController : Controller
    {
        private readonly IConfiguration _configuration;

        public AuditsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("AccessDenied")]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied"); // Отображение страницы с сообщением об отказе в доступе
        }


        [HttpGet("GetAudits")]
        [Authorize(Roles = "admin")]
        public IActionResult GetAudits()
        {
            string connectionString = _configuration.GetConnectionString("SecondConnection");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT event_time, server_principal_name, database_principal_name, database_name, object_name, statement, succeeded FROM sys.fn_get_audit_file('D:\\Audits\\*.sqlaudit', NULL, NULL) ;"; 

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    DataTable dt = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);

                    // Преобразование DataTable в список объектов с выбранными полями
                    List<Audits> auditsList = ConvertDataTableToList(dt);

                    return View("Index", auditsList);
                }
            }
        }
        private List<Audits> ConvertDataTableToList(DataTable dataTable)
        {
            var dataList = new List<Audits>();

            foreach (DataRow row in dataTable.Rows)
            {
                DateTime eventTime;
                if (DateTime.TryParse(row["event_time"].ToString(), out eventTime))
                {
                    if (dataTable.Columns.Contains("database_principal_name"))
                    {
                        var audit = new Audits
                        {
                            event_time = eventTime,
                            server_principal_name = row["server_principal_name"].ToString(),
                            database_principal_name = row["database_principal_name"].ToString(),
                            database_name = row["database_name"].ToString(),
                            object_name = row["object_name"].ToString(),
                            statement = row["statement"].ToString(),
                            succeeded = row["succeeded"].ToString()
                        };
                        bool succeededValue = (bool)row["succeeded"];
                        audit.succeeded = succeededValue ? "Успешно" : "Ошибка";
                        dataList.Add(audit);
                    }

                }
                else
                {
                    // Обработка ситуации, когда строка не может быть сконвертирована в DateTime
                    // Можно вывести ошибку или выполнить другие действия
                }
            }
            return dataList;
        }
    }
}

