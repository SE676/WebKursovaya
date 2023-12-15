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
    public class Документы_о_квалификацииController : Controller
    {
        private readonly MyDBContext _context;

        public Документы_о_квалификацииController(MyDBContext context, IAuditService auditService)
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

        // GET: Документы_о_квалификации
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Документы о квалификации");

            var myDBContext = _context.Документы_о_Квалификации.Include(д => д.Автошкола).Include(д => д.Заявитель).Include(д => д.Категория);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Документы_о_квалификации/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Документы о квалификации");

            if (id == null || _context.Документы_о_Квалификации == null)
            {
                return NotFound();
            }

            var документы_о_квалификации = await _context.Документы_о_Квалификации
                .Include(д => д.Автошкола)
                .Include(д => д.Заявитель)
                .Include(д => д.Категория)
                .FirstOrDefaultAsync(m => m.Номер_документа_о_квалификации == id);
            if (документы_о_квалификации == null)
            {
                return NotFound();
            }

            return View(документы_о_квалификации);
        }

        // GET: Документы_о_квалификации/Create

        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
            
            ViewData["Код_автошколы"] = new SelectList(_context.Автошколы, "Код_автошколы", "Название_автошколы");
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО");
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС");
            return View();
        }




        // POST: Документы_о_квалификации/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Номер_документа_о_квалификации,Код_заявителя,Код_автошколы,Код_категории_ТС,Дата_выдачи")] Документы_о_квалификации документы_о_квалификации)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Документы о квалификации");

            if (ModelState.IsValid)
            {
                _context.Add(документы_о_квалификации);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_автошколы"] = new SelectList(_context.Автошколы, "Код_автошколы", "Код_автошколы", документы_о_квалификации.Код_автошколы);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", документы_о_квалификации.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Код_категории_ТС", документы_о_квалификации.Код_категории_ТС);
            return View(документы_о_квалификации);
        }



        // GET: Документы_о_квалификации/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Документы_о_Квалификации == null)
            {
                return NotFound();
            }

            var документы_о_квалификации = await _context.Документы_о_Квалификации.FindAsync(id);
            if (документы_о_квалификации == null)
            {
                return NotFound();
            }
            ViewData["Код_автошколы"] = new SelectList(_context.Автошколы, "Код_автошколы", "Название_автошколы", документы_о_квалификации.Код_автошколы);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО", документы_о_квалификации.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС", документы_о_квалификации.Код_категории_ТС);
            return View(документы_о_квалификации);
        }

        // POST: Документы_о_квалификации/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Номер_документа_о_квалификации,Код_заявителя,Код_автошколы,Код_категории_ТС,Дата_выдачи")] Документы_о_квалификации документы_о_квалификации)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Документы о квалификации");

            if (id != документы_о_квалификации.Номер_документа_о_квалификации)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(документы_о_квалификации);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Документы_о_квалификацииExists(документы_о_квалификации.Номер_документа_о_квалификации))
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
            ViewData["Код_автошколы"] = new SelectList(_context.Автошколы, "Код_автошколы", "Код_автошколы", документы_о_квалификации.Код_автошколы);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", документы_о_квалификации.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Код_категории_ТС", документы_о_квалификации.Код_категории_ТС);
            return View(документы_о_квалификации);
        }

        // GET: Документы_о_квалификации/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null || _context.Документы_о_Квалификации == null)
            {
                return NotFound();
            }

            var документы_о_квалификации = await _context.Документы_о_Квалификации
                .Include(д => д.Автошкола)
                .Include(д => д.Заявитель)
                .Include(д => д.Категория)
                .FirstOrDefaultAsync(m => m.Номер_документа_о_квалификации == id);
            if (документы_о_квалификации == null)
            {
                return NotFound();
            }

            return View(документы_о_квалификации);
        }

        // POST: Документы_о_квалификации/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Документы о квалификации");

            if (_context.Документы_о_Квалификации == null)
            {
                return Problem("Entity set 'MyDBContext.Документы_о_Квалификации'  is null.");
            }
            var документы_о_квалификации = await _context.Документы_о_Квалификации.FindAsync(id);
            if (документы_о_квалификации != null)
            {
                _context.Документы_о_Квалификации.Remove(документы_о_квалификации);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Документы_о_квалификацииExists(int id)
        {
          return (_context.Документы_о_Квалификации?.Any(e => e.Номер_документа_о_квалификации == id)).GetValueOrDefault();
        }
    }
}
