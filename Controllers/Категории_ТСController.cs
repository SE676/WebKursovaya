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
    public class Категории_ТСController : Controller
    {
        private readonly MyDBContext _context;

        public Категории_ТСController(MyDBContext context, IAuditService auditService)
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

        // GET: Категории_ТС
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Категории ТС");

            return _context.Категории_ТС != null ? 
                          View(await _context.Категории_ТС.ToListAsync()) :
                          Problem("Entity set 'MyDBContext.Категории_ТС'  is null.");
        }

        // GET: Категории_ТС/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Категории ТС");

            if (id == null || _context.Категории_ТС == null)
            {
                return NotFound();
            }

            var категории_ТС = await _context.Категории_ТС
                .FirstOrDefaultAsync(m => m.Код_категории_ТС == id);
            if (категории_ТС == null)
            {
                return NotFound();
            }

            return View(категории_ТС);
        }

        // GET: Категории_ТС/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
           
            return View();
        }

        // POST: Категории_ТС/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_категории_ТС,Наименование_категории_ТС")] Категории_ТС категории_ТС)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Категории ТС");

            if (ModelState.IsValid)
            {
                _context.Add(категории_ТС);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(категории_ТС);
        }

        // GET: Категории_ТС/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Категории_ТС == null)
            {
                return NotFound();
            }

            var категории_ТС = await _context.Категории_ТС.FindAsync(id);
            if (категории_ТС == null)
            {
                return NotFound();
            }
            return View(категории_ТС);
        }

        // POST: Категории_ТС/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_категории_ТС,Наименование_категории_ТС")] Категории_ТС категории_ТС)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Категории ТС");

            if (id != категории_ТС.Код_категории_ТС)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(категории_ТС);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Категории_ТСExists(категории_ТС.Код_категории_ТС))
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
            return View(категории_ТС);
        }

        // GET: Категории_ТС/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
           
            if (id == null || _context.Категории_ТС == null)
            {
                return NotFound();
            }

            var категории_ТС = await _context.Категории_ТС
                .FirstOrDefaultAsync(m => m.Код_категории_ТС == id);
            if (категории_ТС == null)
            {
                return NotFound();
            }

            return View(категории_ТС);
        }

        // POST: Категории_ТС/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Категории ТС");

            if (_context.Категории_ТС == null)
            {
                return Problem("Entity set 'MyDBContext.Категории_ТС'  is null.");
            }
            var категории_ТС = await _context.Категории_ТС.FindAsync(id);
            if (категории_ТС != null)
            {
                _context.Категории_ТС.Remove(категории_ТС);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Категории_ТСExists(int id)
        {
          return (_context.Категории_ТС?.Any(e => e.Код_категории_ТС == id)).GetValueOrDefault();
        }
    }
}
