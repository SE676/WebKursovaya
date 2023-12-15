using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebKursovaya.Data;
using WebKursovaya.Models;

namespace WebKursovaya.Controllers
{
    public class ПосещенияController : Controller
    {
        private readonly MyDBContext _context;

        public ПосещенияController(MyDBContext context)
        {
            _context = context;

        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied"); // Отображение страницы с сообщением об отказе в доступе
        }

        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            return _context.Посещения != null ?
                         View(await _context.Посещения.ToListAsync()) :
                         Problem("Entity set 'MyDBContext.Посещения'  is null.");
        }
    }
}
