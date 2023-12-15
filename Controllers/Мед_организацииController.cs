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
    public class Мед_организацииController : Controller
    {
        private readonly MyDBContext _context;

        public Мед_организацииController(MyDBContext context, IAuditService auditService)
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

        // GET: Мед_организации
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Медицинские организации");

            return _context.Мед_организации != null ? 
                          View(await _context.Мед_организации.ToListAsync()) :
                          Problem("Entity set 'MyDBContext.Мед_организации'  is null.");
        }

        // GET: Мед_организации/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Медицинские организации");

            if (id == null || _context.Мед_организации == null)
            {
                return NotFound();
            }

            var мед_организации = await _context.Мед_организации
                .FirstOrDefaultAsync(m => m.Код_медицинской_организации == id);
            if (мед_организации == null)
            {
                return NotFound();
            }

            return View(мед_организации);
        }

        // GET: Мед_организации/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Мед_организации/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_медицинской_организации,Название_медицинской_организации,Адрес_медицинской_организации,Email_медицинской_организации")] Мед_организации мед_организации)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Медицинские организации");

            if (ModelState.IsValid)
            {
                _context.Add(мед_организации);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(мед_организации);
        }

        // GET: Мед_организации/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Мед_организации == null)
            {
                return NotFound();
            }

            var мед_организации = await _context.Мед_организации.FindAsync(id);
            if (мед_организации == null)
            {
                return NotFound();
            }
            return View(мед_организации);
        }

        // POST: Мед_организации/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_медицинской_организации,Название_медицинской_организации,Адрес_медицинской_организации,Email_медицинской_организации")] Мед_организации мед_организации)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Медицинские организации");

            if (id != мед_организации.Код_медицинской_организации)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(мед_организации);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Мед_организацииExists(мед_организации.Код_медицинской_организации))
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
            return View(мед_организации);
        }

        // GET: Мед_организации/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
           

            if (id == null || _context.Мед_организации == null)
            {
                return NotFound();
            }

            var мед_организации = await _context.Мед_организации
                .FirstOrDefaultAsync(m => m.Код_медицинской_организации == id);
            if (мед_организации == null)
            {
                return NotFound();
            }

            return View(мед_организации);
        }

        // POST: Мед_организации/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Медицинские организации");

            if (_context.Мед_организации == null)
            {
                return Problem("Entity set 'MyDBContext.Мед_организации'  is null.");
            }
            var мед_организации = await _context.Мед_организации.FindAsync(id);
            if (мед_организации != null)
            {
                _context.Мед_организации.Remove(мед_организации);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Мед_организацииExists(int id)
        {
          return (_context.Мед_организации?.Any(e => e.Код_медицинской_организации == id)).GetValueOrDefault();
        }
    }
}
