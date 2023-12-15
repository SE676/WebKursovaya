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
    public class ПаспортаController : Controller
    {
        private readonly MyDBContext _context;

        public ПаспортаController(MyDBContext context, IAuditService auditService)
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


        // GET: Паспорта
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Паспорта");

            var myDBContext = _context.Паспорта.Include(п => п.Заявитель);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Паспорта/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Паспорта");

            if (id == null || _context.Паспорта == null)
            {
                return NotFound();
            }

            var паспорта = await _context.Паспорта
                .Include(п => п.Заявитель)
                .FirstOrDefaultAsync(m => m.Код_паспорта == id);
            if (паспорта == null)
            {
                return NotFound();
            }

            return View(паспорта);
        }

        // GET: Паспорта/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
            

            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО");
            return View();
        }

        // POST: Паспорта/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_паспорта,Код_заявителя,Серия_паспорта,Номер_паспорта,Дата_выдачи,Кем_выдан,Адрес_прописки")] Паспорта паспорта)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Паспорта");

            if (ModelState.IsValid)
            {
                _context.Add(паспорта);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", паспорта.Код_заявителя);
            return View(паспорта);
        }

        // GET: Паспорта/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {        

            if (id == null || _context.Паспорта == null)
            {
                return NotFound();
            }

            var паспорта = await _context.Паспорта.FindAsync(id);
            if (паспорта == null)
            {
                return NotFound();
            }
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО", паспорта.Код_заявителя);
            return View(паспорта);
        }

        // POST: Паспорта/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_паспорта,Код_заявителя,Серия_паспорта,Номер_паспорта,Дата_выдачи,Кем_выдан,Адрес_прописки")] Паспорта паспорта)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Паспорта");

            if (id != паспорта.Код_паспорта)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(паспорта);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПаспортаExists(паспорта.Код_паспорта))
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
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", паспорта.Код_заявителя);
            return View(паспорта);
        }

        // GET: Паспорта/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
           

            if (id == null || _context.Паспорта == null)
            {
                return NotFound();
            }

            var паспорта = await _context.Паспорта
                .Include(п => п.Заявитель)
                .FirstOrDefaultAsync(m => m.Код_паспорта == id);
            if (паспорта == null)
            {
                return NotFound();
            }

            return View(паспорта);
        }

        // POST: Паспорта/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Паспорта");

            if (_context.Паспорта == null)
            {
                return Problem("Entity set 'MyDBContext.Паспорта'  is null.");
            }
            var паспорта = await _context.Паспорта.FindAsync(id);
            if (паспорта != null)
            {
                _context.Паспорта.Remove(паспорта);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ПаспортаExists(int id)
        {
          return (_context.Паспорта?.Any(e => e.Код_паспорта == id)).GetValueOrDefault();
        }
    }
}
