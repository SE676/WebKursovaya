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
    public class АвтошколыController : Controller
    {
        private readonly MyDBContext _context;

        public АвтошколыController(MyDBContext context, IAuditService auditService)
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


        // GET: Автошколы
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Автошколы");

            return _context.Автошколы != null ? 
                          View(await _context.Автошколы.ToListAsync()) :
                          Problem("Entity set 'MyDBContext.Автошколы'  is null.");
        }

        // GET: Автошколы/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Автошколы");

            if (id == null || _context.Автошколы == null)
            {
                return NotFound();
            }

            var автошколы = await _context.Автошколы
                .FirstOrDefaultAsync(m => m.Код_автошколы == id);
            if (автошколы == null)
            {
                return NotFound();
            }

            return View(автошколы);
        }

        // GET: Автошколы/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {            
            return View();
        }


        // POST: Автошколы/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_автошколы,Название_автошколы,Адрес_автошколы,Email_автошколы")] Автошколы автошколы)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Автошколы");

            if (ModelState.IsValid)
            {
                _context.Add(автошколы);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(автошколы);
        }

        // GET: Автошколы/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Автошколы == null)
            {
                return NotFound();
            }

            var автошколы = await _context.Автошколы.FindAsync(id);
            if (автошколы == null)
            {
                return NotFound();
            }
            return View(автошколы);
        }

        // POST: Автошколы/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_автошколы,Название_автошколы,Адрес_автошколы,Email_автошколы")] Автошколы автошколы)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Автошколы");

            if (id != автошколы.Код_автошколы)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(автошколы);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!АвтошколыExists(автошколы.Код_автошколы))
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
            return View(автошколы);
        }

        // GET: Автошколы/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null || _context.Автошколы == null)
            {
                return NotFound();
            }

            var автошколы = await _context.Автошколы
                .FirstOrDefaultAsync(m => m.Код_автошколы == id);
            if (автошколы == null)
            {
                return NotFound();
            }

            return View(автошколы);
        }

        // POST: Автошколы/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Автошколы");

            if (_context.Автошколы == null)
            {
                return Problem("Entity set 'MyDBContext.Автошколы'  is null.");
            }
            var автошколы = await _context.Автошколы.FindAsync(id);
            if (автошколы != null)
            {
                _context.Автошколы.Remove(автошколы);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool АвтошколыExists(int id)
        {
          return (_context.Автошколы?.Any(e => e.Код_автошколы == id)).GetValueOrDefault();
        }
    }
}
