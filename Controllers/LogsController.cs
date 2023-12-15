using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebKursovaya.Models;

namespace WebKursovaya.Controllers
{
    public class LogsController : Controller
    {
        private readonly UserContext _dbContext;
        private readonly ILogger<LogsController> _logger;
        public LogsController(UserContext dbContext, ILogger<LogsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        [HttpGet("Index")]
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            try
            {
                var auditLogs = _dbContext.Logs
            .Where(log => log.Имя_пользователя != null && log.Таблица != null && log.Роль != null)
            .ToList(); // Замените YourProperty на конкретное свойство, где могут быть NULL значения

                // Логгирование информации о запросе логов
                _logger.LogInformation("Запрошены аудит-логи");

                return View(auditLogs);
            }
            catch (Exception ex)
            {
                // Логгирование ошибки
                _logger.LogError(ex, "Ошибка при запросе логов");
                return RedirectToAction("Index"); // Или любая другая обработка ошибки
            }
        }
    }
}

