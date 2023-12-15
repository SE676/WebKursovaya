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
    public class ВУ_Категории_ТСController : Controller
    {
        private readonly MyDBContext _context;

        public ВУ_Категории_ТСController(MyDBContext context, IAuditService auditService)
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

        // GET: ВУ_Категории_ТС
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ВУ Категории ТС");

            var myDBContext = _context.ВУ_Категории_ТС.Include(в => в.ВУ).Include(в => в.Категория);
            return View(await myDBContext.ToListAsync());
        }

        // GET: ВУ_Категории_ТС/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ВУ Категории ТС");

            if (id == null || _context.ВУ_Категории_ТС == null)
            {
                return NotFound();
            }

            var вУ_Категории_ТС = await _context.ВУ_Категории_ТС
                .Include(в => в.ВУ)
                .Include(в => в.Категория)
                .FirstOrDefaultAsync(m => m.Код == id);
            if (вУ_Категории_ТС == null)
            {
                return NotFound();
            }

            return View(вУ_Категории_ТС);
        }

        // GET: ВУ_Категории_ТС/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
            
            ViewData["Код_ВУ"] = new SelectList(_context.ВУ, "Код_ВУ", "ВУсн");
            ViewData["Код_Категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС");
            return View();
        }

        // POST: ВУ_Категории_ТС/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код,Код_ВУ,Код_Категории_ТС")] ВУ_Категории_ТС вУ_Категории_ТС)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "ВУ Категории ТС");

            if (ModelState.IsValid)
            {
                _context.Add(вУ_Категории_ТС);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_ВУ"] = new SelectList(_context.ВУ, "Код_ВУ", "Код_ВУ", вУ_Категории_ТС.Код_ВУ);
            ViewData["Код_Категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС", вУ_Категории_ТС.Код_Категории_ТС);
            return View(вУ_Категории_ТС);
        }

        // GET: ВУ_Категории_ТС/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.ВУ_Категории_ТС == null)
            {
                return NotFound();
            }

            var вУ_Категории_ТС = await _context.ВУ_Категории_ТС.FindAsync(id);
            if (вУ_Категории_ТС == null)
            {
                return NotFound();
            }
            ViewData["Код_ВУ"] = new SelectList(_context.ВУ, "Код_ВУ", "ВУсн", вУ_Категории_ТС.Код_ВУ);
            ViewData["Код_Категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС", вУ_Категории_ТС.Код_Категории_ТС);
            return View(вУ_Категории_ТС);
        }

        // POST: ВУ_Категории_ТС/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код,Код_ВУ,Код_Категории_ТС")] ВУ_Категории_ТС вУ_Категории_ТС)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "ВУ Категории ТС");

            if (id != вУ_Категории_ТС.Код)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(вУ_Категории_ТС);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ВУ_Категории_ТСExists(вУ_Категории_ТС.Код))
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
            ViewData["Код_ВУ"] = new SelectList(_context.ВУ, "Код_ВУ", "Код_ВУ", вУ_Категории_ТС.Код_ВУ);
            ViewData["Код_Категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС", вУ_Категории_ТС.Код_Категории_ТС);
            return View(вУ_Категории_ТС);
        }

        // GET: ВУ_Категории_ТС/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null || _context.ВУ_Категории_ТС == null)
            {
                return NotFound();
            }

            var вУ_Категории_ТС = await _context.ВУ_Категории_ТС
                .Include(в => в.ВУ)
                .Include(в => в.Категория)
                .FirstOrDefaultAsync(m => m.Код == id);
            if (вУ_Категории_ТС == null)
            {
                return NotFound();
            }

            return View(вУ_Категории_ТС);
        }

        // POST: ВУ_Категории_ТС/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "ВУ Категории ТС");

            if (_context.ВУ_Категории_ТС == null)
            {
                return Problem("Entity set 'MyDBContext.ВУ_Категории_ТС'  is null.");
            }
            var вУ_Категории_ТС = await _context.ВУ_Категории_ТС.FindAsync(id);
            if (вУ_Категории_ТС != null)
            {
                _context.ВУ_Категории_ТС.Remove(вУ_Категории_ТС);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ВУ_Категории_ТСExists(int id)
        {
          return (_context.ВУ_Категории_ТС?.Any(e => e.Код == id)).GetValueOrDefault();
        }
    }
}
