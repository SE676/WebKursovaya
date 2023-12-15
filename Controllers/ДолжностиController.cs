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
    public class ДолжностиController : Controller
    {
        private readonly MyDBContext _context;

        public ДолжностиController(MyDBContext context, IAuditService auditService)
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


        // GET: Должности
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Должности");

            return _context.Должности != null ? 
                          View(await _context.Должности.ToListAsync()) :
                          Problem("Entity set 'MyDBContext.Должности'  is null.");
        }

        // GET: Должности/Details/5
        [Authorize(Roles = "leader, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Должности");

            if (id == null || _context.Должности == null)
            {
                return NotFound();
            }

            var должности = await _context.Должности
                .FirstOrDefaultAsync(m => m.Код_должности == id);
            if (должности == null)
            {
                return NotFound();
            }

            return View(должности);
        }

        // GET: Должности/Create
        [Authorize(Roles = "leader, admin")]
        public IActionResult Create()
        {
          
            return View();
        }

        // POST: Должности/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_должности,Название_должности")] Должности должности)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Должности");

            if (ModelState.IsValid)
            {
                _context.Add(должности);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(должности);
        }

        // GET: Должности/Edit/5
        [Authorize(Roles = "leader, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Должности == null)
            {
                return NotFound();
            }

            var должности = await _context.Должности.FindAsync(id);
            if (должности == null)
            {
                return NotFound();
            }
            return View(должности);
        }

        // POST: Должности/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_должности,Название_должности")] Должности должности)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Должности");

            if (id != должности.Код_должности)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(должности);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ДолжностиExists(должности.Код_должности))
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
            return View(должности);
        }

        // GET: Должности/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
 

            if (id == null || _context.Должности == null)
            {
                return NotFound();
            }

            var должности = await _context.Должности
                .FirstOrDefaultAsync(m => m.Код_должности == id);
            if (должности == null)
            {
                return NotFound();
            }

            return View(должности);
        }

        // POST: Должности/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Должности");


            if (_context.Должности == null)
            {
                return Problem("Entity set 'MyDBContext.Должности'  is null.");
            }
            var должности = await _context.Должности.FindAsync(id);
            if (должности != null)
            {
                _context.Должности.Remove(должности);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ДолжностиExists(int id)
        {
          return (_context.Должности?.Any(e => e.Код_должности == id)).GetValueOrDefault();
        }
    }
}
