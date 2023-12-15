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
    public class Виды_статусов_ВУController : Controller
    {
        private readonly MyDBContext _context;

        public Виды_статусов_ВУController(MyDBContext context, IAuditService auditService)
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

        // GET: Виды_статусов_ВУ
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Статусы ВУ");

            return _context.Виды_статусов_ВУ != null ? 
                          View(await _context.Виды_статусов_ВУ.ToListAsync()) :
                          Problem("Entity set 'MyDBContext.Виды_статусов_ВУ'  is null.");
        }

        // GET: Виды_статусов_ВУ/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Статусы ВУ");


            if (id == null || _context.Виды_статусов_ВУ == null)
            {
                return NotFound();
            }

            var виды_статусов_ВУ = await _context.Виды_статусов_ВУ
                .FirstOrDefaultAsync(m => m.Код_статуса_ВУ == id);
            if (виды_статусов_ВУ == null)
            {
                return NotFound();
            }

            return View(виды_статусов_ВУ);
        }

        // GET: Виды_статусов_ВУ/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Виды_статусов_ВУ/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_статуса_ВУ,Название_статуса")] Виды_статусов_ВУ виды_статусов_ВУ)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Статусы ВУ");

            if (ModelState.IsValid)
            {
                _context.Add(виды_статусов_ВУ);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(виды_статусов_ВУ);
        }

        // GET: Виды_статусов_ВУ/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            

            if (id == null || _context.Виды_статусов_ВУ == null)
            {
                return NotFound();
            }

            var виды_статусов_ВУ = await _context.Виды_статусов_ВУ.FindAsync(id);
            if (виды_статусов_ВУ == null)
            {
                return NotFound();
            }
            return View(виды_статусов_ВУ);
        }

        // POST: Виды_статусов_ВУ/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_статуса_ВУ,Название_статуса")] Виды_статусов_ВУ виды_статусов_ВУ)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Статусы ВУ");

            if (id != виды_статусов_ВУ.Код_статуса_ВУ)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(виды_статусов_ВУ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Виды_статусов_ВУExists(виды_статусов_ВУ.Код_статуса_ВУ))
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
            return View(виды_статусов_ВУ);
        }

        // GET: Виды_статусов_ВУ/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null || _context.Виды_статусов_ВУ == null)
            {
                return NotFound();
            }

            var виды_статусов_ВУ = await _context.Виды_статусов_ВУ
                .FirstOrDefaultAsync(m => m.Код_статуса_ВУ == id);
            if (виды_статусов_ВУ == null)
            {
                return NotFound();
            }

            return View(виды_статусов_ВУ);
        }

        // POST: Виды_статусов_ВУ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Статусы ВУ");

            if (_context.Виды_статусов_ВУ == null)
            {
                return Problem("Entity set 'MyDBContext.Виды_статусов_ВУ'  is null.");
            }
            var виды_статусов_ВУ = await _context.Виды_статусов_ВУ.FindAsync(id);
            if (виды_статусов_ВУ != null)
            {
                _context.Виды_статусов_ВУ.Remove(виды_статусов_ВУ);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Виды_статусов_ВУExists(int id)
        {
          return (_context.Виды_статусов_ВУ?.Any(e => e.Код_статуса_ВУ == id)).GetValueOrDefault();
        }
    }
}
