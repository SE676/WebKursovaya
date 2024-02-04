using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebKursovaya.Data;
using WebKursovaya.Models;

namespace WebKursovaya.Controllers
{
    public class TestASController : Controller
    {
        private readonly MyDBContext _context;

        public TestASController(MyDBContext context)
        {
            _context = context;

        }

        [HttpGet]
        
        public IActionResult AccessDenied()
        {
            return View("AccessDenied"); // Отображение страницы с сообщением об отказе в доступе
        }

        
        public async Task<IActionResult> Index()
        {
            return _context.TestAS != null ?
                         View(await _context.TestAS.ToListAsync()) :
                         Problem("Entity set 'MyDBContext.TestAS'  is null.");
        }
    }
}
