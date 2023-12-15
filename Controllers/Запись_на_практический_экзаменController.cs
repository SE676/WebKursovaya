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
    public class Запись_на_практический_экзаменController : Controller
    {
        private readonly MyDBContext _context;

        public Запись_на_практический_экзаменController(MyDBContext context, IAuditService auditService)
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


        // GET: Запись_на_практический_экзамен
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Запись на практический экзамен");

            var myDBContext = _context.Запись_на_практический_экзамен.Include(з => з.Заявитель).Include(з => з.Категория);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Запись_на_практический_экзамен/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Запись на практический экзамен");

            if (id == null || _context.Запись_на_практический_экзамен == null)
            {
                return NotFound();
            }

            var запись_на_практический_экзамен = await _context.Запись_на_практический_экзамен
                .Include(з => з.Заявитель)
                .Include(з => з.Категория)
                .FirstOrDefaultAsync(m => m.Код_записи == id);
            if (запись_на_практический_экзамен == null)
            {
                return NotFound();
            }

            return View(запись_на_практический_экзамен);
        }

        // GET: Запись_на_практический_экзамен/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
          
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО");
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС");
            return View();
        }

        // POST: Запись_на_практический_экзамен/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_записи,Дата_записи,Код_заявителя,Код_категории_ТС")] Запись_на_практический_экзамен запись_на_практический_экзамен)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Запись на практический экзамен");

            if (ModelState.IsValid)
            {
                _context.Add(запись_на_практический_экзамен);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", запись_на_практический_экзамен.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Код_категории_ТС", запись_на_практический_экзамен.Код_категории_ТС);
            return View(запись_на_практический_экзамен);
        }

        // GET: Запись_на_практический_экзамен/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null || _context.Запись_на_практический_экзамен == null)
            {
                return NotFound();
            }

            var запись_на_практический_экзамен = await _context.Запись_на_практический_экзамен.FindAsync(id);
            if (запись_на_практический_экзамен == null)
            {
                return NotFound();
            }
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО", запись_на_практический_экзамен.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС", запись_на_практический_экзамен.Код_категории_ТС);
            return View(запись_на_практический_экзамен);
        }

        // POST: Запись_на_практический_экзамен/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_записи,Дата_записи,Код_заявителя,Код_категории_ТС")] Запись_на_практический_экзамен запись_на_практический_экзамен)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Запись на практический экзамен");

            if (id != запись_на_практический_экзамен.Код_записи)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(запись_на_практический_экзамен);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Запись_на_практический_экзаменExists(запись_на_практический_экзамен.Код_записи))
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
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", запись_на_практический_экзамен.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Код_категории_ТС", запись_на_практический_экзамен.Код_категории_ТС);
            return View(запись_на_практический_экзамен);
        }

        // GET: Запись_на_практический_экзамен/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null || _context.Запись_на_практический_экзамен == null)
            {
                return NotFound();
            }

            var запись_на_практический_экзамен = await _context.Запись_на_практический_экзамен
                .Include(з => з.Заявитель)
                .Include(з => з.Категория)
                .FirstOrDefaultAsync(m => m.Код_записи == id);
            if (запись_на_практический_экзамен == null)
            {
                return NotFound();
            }

            return View(запись_на_практический_экзамен);
        }

        // POST: Запись_на_практический_экзамен/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Запись на практический экзамен");

            if (_context.Запись_на_практический_экзамен == null)
            {
                return Problem("Entity set 'MyDBContext.Запись_на_практический_экзамен'  is null.");
            }
            var запись_на_практический_экзамен = await _context.Запись_на_практический_экзамен.FindAsync(id);
            if (запись_на_практический_экзамен != null)
            {
                _context.Запись_на_практический_экзамен.Remove(запись_на_практический_экзамен);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Запись_на_практический_экзаменExists(int id)
        {
          return (_context.Запись_на_практический_экзамен?.Any(e => e.Код_записи == id)).GetValueOrDefault();
        }
    }
}
