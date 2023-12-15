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
    public class Стоимость_госпошлиныController : Controller
    {
        private readonly MyDBContext _context;

        public Стоимость_госпошлиныController(MyDBContext context, IAuditService auditService)
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

        // GET: Стоимость_госпошлины
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Стоимость госпошлины");

            return _context.Стоимость_госпошлины != null ? 
                          View(await _context.Стоимость_госпошлины.ToListAsync()) :
                          Problem("Entity set 'MyDBContext.Стоимость_госпошлины'  is null.");
        }

        // GET: Стоимость_госпошлины/Details/5
        [Authorize(Roles = "fd, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Стоимость госпошлины");

            if (id == null || _context.Стоимость_госпошлины == null)
            {
                return NotFound();
            }

            var стоимость_госпошлины = await _context.Стоимость_госпошлины
                .FirstOrDefaultAsync(m => m.Код_стоимости == id);
            if (стоимость_госпошлины == null)
            {
                return NotFound();
            }

            return View(стоимость_госпошлины);
        }

        // GET: Стоимость_госпошлины/Create
        [Authorize(Roles = "fd, admin")]
        public IActionResult Create()
        {

            return View();
        }

        // POST: Стоимость_госпошлины/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_стоимости,Дата,Стоимость")] Стоимость_госпошлины стоимость_госпошлины)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Стоимость госпошлины");

            if (ModelState.IsValid)
            {
                _context.Add(стоимость_госпошлины);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(стоимость_госпошлины);
        }

        // GET: Стоимость_госпошлины/Edit/5
        [Authorize(Roles = "fd, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            

            if (id == null || _context.Стоимость_госпошлины == null)
            {
                return NotFound();
            }

            var стоимость_госпошлины = await _context.Стоимость_госпошлины.FindAsync(id);
            if (стоимость_госпошлины == null)
            {
                return NotFound();
            }
            return View(стоимость_госпошлины);
        }

        // POST: Стоимость_госпошлины/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_стоимости,Дата,Стоимость")] Стоимость_госпошлины стоимость_госпошлины)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Стоимость госпошлины");

            if (id != стоимость_госпошлины.Код_стоимости)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(стоимость_госпошлины);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Стоимость_госпошлиныExists(стоимость_госпошлины.Код_стоимости))
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
            return View(стоимость_госпошлины);
        }

        // GET: Стоимость_госпошлины/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            

            if (id == null || _context.Стоимость_госпошлины == null)
            {
                return NotFound();
            }

            var стоимость_госпошлины = await _context.Стоимость_госпошлины
                .FirstOrDefaultAsync(m => m.Код_стоимости == id);
            if (стоимость_госпошлины == null)
            {
                return NotFound();
            }

            return View(стоимость_госпошлины);
        }

        // POST: Стоимость_госпошлины/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Стоимость госпошлины");

            if (_context.Стоимость_госпошлины == null)
            {
                return Problem("Entity set 'MyDBContext.Стоимость_госпошлины'  is null.");
            }
            var стоимость_госпошлины = await _context.Стоимость_госпошлины.FindAsync(id);
            if (стоимость_госпошлины != null)
            {
                _context.Стоимость_госпошлины.Remove(стоимость_госпошлины);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Стоимость_госпошлиныExists(int id)
        {
          return (_context.Стоимость_госпошлины?.Any(e => e.Код_стоимости == id)).GetValueOrDefault();
        }
    }
}
