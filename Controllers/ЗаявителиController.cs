using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebKursovaya.Data;
using WebKursovaya.Models;

namespace WebKursovaya.Controllers
{
    public class ЗаявителиController : Controller
    {
        private readonly MyDBContext _context;
        public ЗаявителиController(MyDBContext context, IAuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        private readonly IAuditService _auditService;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied"); // Отображение страницы с сообщением об отказе в доступе
        }


        // GET: Заявители
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Заявители");
    
            var myDBContext = _context.Заявители.Include(з => з.Пол);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Заявители/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Заявители");

            if (id == null || _context.Заявители == null)
            {
                return NotFound();
            }

            var заявители = await _context.Заявители
                .Include(з => з.Пол)
                .FirstOrDefaultAsync(m => m.Код_заявителя == id);
            if (заявители == null)
            {
                return NotFound();
            }

            return View(заявители);
        }

        // GET: Заявители/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create(int id)
        {
           
            //ViewData["Код_пола"] = new SelectList(_context.Полы, "Код_пола", "Пол");
            //return View();

            ViewData["Код_пола"] = new SelectList(_context.Полы, "Код_пола", "Пол");

            Посещения посещение = _context.Посещения.FirstOrDefault(p => p.Код_посещения == id);

            if (посещение != null)
            {
                // Создаем новый объект заявителя и заполняем его данными о посещении
                Заявители заявитель = new Заявители
                {
                    Фамилия = посещение.Фамилия,
                    Имя = посещение.Имя,
                    Отчество = посещение.Отчество,
                    Дата_рождения = посещение.Дата_рождения

                    // Заполните другие поля заявителя, если они есть и могут быть извлечены из посещения
                };

                // Отправляем данные заявителя в представление Create заявителя
                return View(заявитель);
            }
            else
            {
                // Если данные о посещении не найдены, выполните необходимые действия или верните сообщение об ошибке
                return View();
            }


        }

        // POST: Заявители/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_заявителя,Фамилия,Имя,Отчество,Дата_рождения,Код_пола")] Заявители заявители)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Заявители");

            if (ModelState.IsValid)
            {
                _context.Add(заявители);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_пола"] = new SelectList(_context.Полы, "Код_пола", "Код_пола", заявители.Код_пола);
            return View(заявители);
        }

        // GET: Заявители/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            

            if (id == null || _context.Заявители == null)
            {
                return NotFound();
            }

            var заявители = await _context.Заявители.FindAsync(id);
            if (заявители == null)
            {
                return NotFound();
            }
            ViewData["Код_пола"] = new SelectList(_context.Полы, "Код_пола", "Пол", заявители.Код_пола);
            return View(заявители);
        }

        // POST: Заявители/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_заявителя,Фамилия,Имя,Отчество,Дата_рождения,Код_пола")] Заявители заявители)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Заявители");

            if (id != заявители.Код_заявителя)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(заявители);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ЗаявителиExists(заявители.Код_заявителя))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_пола"] = new SelectList(_context.Полы, "Код_пола", "Код_пола", заявители.Код_пола);
            return View(заявители);
        }

        // GET: Заявители/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
           

            if (id == null || _context.Заявители == null)
            {
                return NotFound();
            }

            var заявители = await _context.Заявители
                .Include(з => з.Пол)
                .FirstOrDefaultAsync(m => m.Код_заявителя == id);
            if (заявители == null)
            {
                return NotFound();
            }

            return View(заявители);
        }

        // POST: Заявители/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Заявители");

            if (_context.Заявители == null)
            {
                return Problem("Entity set 'MyDBContext.Заявители'  is null.");
            }
            var заявители = await _context.Заявители.FindAsync(id);
            if (заявители != null)
            {
                _context.Заявители.Remove(заявители);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ЗаявителиExists(int id)
        {
          return (_context.Заявители?.Any(e => e.Код_заявителя == id)).GetValueOrDefault();
        }
    }
}
