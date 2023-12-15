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
    public class Виды_услугController : Controller
    {
        private readonly MyDBContext _context;

        public Виды_услугController(MyDBContext context, IAuditService auditService)
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

        // GET: Виды_услуг
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Виды услуг");

            return _context.Виды_услуг != null ? 
                          View(await _context.Виды_услуг.ToListAsync()) :
                          Problem("Entity set 'MyDBContext.Виды_услуг'  is null.");
        }

        // GET: Виды_услуг/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Виды услуг");


            if (id == null || _context.Виды_услуг == null)
            {
                return NotFound();
            }

            var виды_услуг = await _context.Виды_услуг
                .FirstOrDefaultAsync(m => m.Код_вида_услуги == id);
            if (виды_услуг == null)
            {
                return NotFound();
            }

            return View(виды_услуг);
        }

        // GET: Виды_услуг/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {         
            return View();
        }

        // POST: Виды_услуг/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_вида_услуги,Название_вида_услуги")] Виды_услуг виды_услуг)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Виды услуг");

            if (ModelState.IsValid)
            {
                _context.Add(виды_услуг);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(виды_услуг);
        }

        // GET: Виды_услуг/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
           

            if (id == null || _context.Виды_услуг == null)
            {
                return NotFound();
            }

            var виды_услуг = await _context.Виды_услуг.FindAsync(id);
            if (виды_услуг == null)
            {
                return NotFound();
            }
            return View(виды_услуг);
        }

        // POST: Виды_услуг/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_вида_услуги,Название_вида_услуги")] Виды_услуг виды_услуг)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Виды услуг");

            if (id != виды_услуг.Код_вида_услуги)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(виды_услуг);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Виды_услугExists(виды_услуг.Код_вида_услуги))
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
            return View(виды_услуг);
        }

        // GET: Виды_услуг/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null || _context.Виды_услуг == null)
            {
                return NotFound();
            }

            var виды_услуг = await _context.Виды_услуг
                .FirstOrDefaultAsync(m => m.Код_вида_услуги == id);
            if (виды_услуг == null)
            {
                return NotFound();
            }

            return View(виды_услуг);
        }

        // POST: Виды_услуг/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Виды услуг");

            if (_context.Виды_услуг == null)
            {
                return Problem("Entity set 'MyDBContext.Виды_услуг'  is null.");
            }
            var виды_услуг = await _context.Виды_услуг.FindAsync(id);
            if (виды_услуг != null)
            {
                _context.Виды_услуг.Remove(виды_услуг);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Виды_услугExists(int id)
        {
          return (_context.Виды_услуг?.Any(e => e.Код_вида_услуги == id)).GetValueOrDefault();
        }
    }
}
