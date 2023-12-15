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
    public class Формы_003_В_уController : Controller
    {
        private readonly MyDBContext _context;

        public Формы_003_В_уController(MyDBContext context, IAuditService auditService)
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

        // GET: Формы_003_В_у
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Мед. справки");

            var myDBContext = _context.Формы_003_В_у.Include(ф => ф.Заявитель).Include(ф => ф.Категория).Include(ф => ф.Мед_организация);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Формы_003_В_у/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Мед. справки");

            if (id == null || _context.Формы_003_В_у == null)
            {
                return NotFound();
            }

            var формы_003_В_у = await _context.Формы_003_В_у
                .Include(ф => ф.Заявитель)
                .Include(ф => ф.Категория)
                .Include(ф => ф.Мед_организация)
                .FirstOrDefaultAsync(m => m.Номер_формы_003_В_у == id);
            if (формы_003_В_у == null)
            {
                return NotFound();
            }

            return View(формы_003_В_у);
        }

        // GET: Формы_003_В_у/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
           
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО");
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС");
            ViewData["Код_медицинской_организации"] = new SelectList(_context.Мед_организации, "Код_медицинской_организации", "Название_медицинской_организации");
            return View();
        }

        // POST: Формы_003_В_у/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Номер_формы_003_В_у,Код_заявителя,Код_медицинской_организации,Код_категории_ТС,Дата_выдачи,Сведения_о_медицинских_противопоказаниях")] Формы_003_В_у формы_003_В_у)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Мед. справки");

            if (ModelState.IsValid)
            {
                _context.Add(формы_003_В_у);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", формы_003_В_у.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Код_категории_ТС", формы_003_В_у.Код_категории_ТС);
            ViewData["Код_медицинской_организации"] = new SelectList(_context.Мед_организации, "Код_медицинской_организации", "Код_медицинской_организации", формы_003_В_у.Код_медицинской_организации);
            return View(формы_003_В_у);
        }

        // GET: Формы_003_В_у/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            

            if (id == null || _context.Формы_003_В_у == null)
            {
                return NotFound();
            }

            var формы_003_В_у = await _context.Формы_003_В_у.FindAsync(id);
            if (формы_003_В_у == null)
            {
                return NotFound();
            }
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО", формы_003_В_у.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС", формы_003_В_у.Код_категории_ТС);
            ViewData["Код_медицинской_организации"] = new SelectList(_context.Мед_организации, "Код_медицинской_организации", "Название_медицинской_организации", формы_003_В_у.Код_медицинской_организации);
            return View(формы_003_В_у);
        }

        // POST: Формы_003_В_у/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Номер_формы_003_В_у,Код_заявителя,Код_медицинской_организации,Код_категории_ТС,Дата_выдачи,Сведения_о_медицинских_противопоказаниях")] Формы_003_В_у формы_003_В_у)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Мед. справки");

            if (id != формы_003_В_у.Номер_формы_003_В_у)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(формы_003_В_у);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Формы_003_В_уExists(формы_003_В_у.Номер_формы_003_В_у))
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
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", формы_003_В_у.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Код_категории_ТС", формы_003_В_у.Код_категории_ТС);
            ViewData["Код_медицинской_организации"] = new SelectList(_context.Мед_организации, "Код_медицинской_организации", "Код_медицинской_организации", формы_003_В_у.Код_медицинской_организации);
            return View(формы_003_В_у);
        }

        // GET: Формы_003_В_у/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
           

            if (id == null || _context.Формы_003_В_у == null)
            {
                return NotFound();
            }

            var формы_003_В_у = await _context.Формы_003_В_у
                .Include(ф => ф.Заявитель)
                .Include(ф => ф.Категория)
                .Include(ф => ф.Мед_организация)
                .FirstOrDefaultAsync(m => m.Номер_формы_003_В_у == id);
            if (формы_003_В_у == null)
            {
                return NotFound();
            }

            return View(формы_003_В_у);
        }

        // POST: Формы_003_В_у/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Мед. справки");

            if (_context.Формы_003_В_у == null)
            {
                return Problem("Entity set 'MyDBContext.Формы_003_В_у'  is null.");
            }
            var формы_003_В_у = await _context.Формы_003_В_у.FindAsync(id);
            if (формы_003_В_у != null)
            {
                _context.Формы_003_В_у.Remove(формы_003_В_у);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Формы_003_В_уExists(int id)
        {
          return (_context.Формы_003_В_у?.Any(e => e.Номер_формы_003_В_у == id)).GetValueOrDefault();
        }
    }
}
