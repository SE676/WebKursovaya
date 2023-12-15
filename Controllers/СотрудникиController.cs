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
    public class СотрудникиController : Controller
    {
        private readonly MyDBContext _context;

        public СотрудникиController(MyDBContext context, IAuditService auditService)
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

        // GET: Сотрудники
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Сотрудники");

            var myDBContext = _context.Сотрудники.Include(с => с.Должность);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Сотрудники/Details/5
        [Authorize(Roles = "leader, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Сотрудники");

            if (id == null || _context.Сотрудники == null)
            {
                return NotFound();
            }

            var сотрудники = await _context.Сотрудники
                .Include(с => с.Должность)
                .FirstOrDefaultAsync(m => m.Код_сотрудника == id);
            if (сотрудники == null)
            {
                return NotFound();
            }

            return View(сотрудники);
        }

        // GET: Сотрудники/Create
        [Authorize(Roles = "leader, admin")]
        public IActionResult Create()
        {
            
            ViewData["Код_должности"] = new SelectList(_context.Должности, "Код_должности", "Название_должности");
            return View();
        }

        // POST: Сотрудники/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_сотрудника,Фамилия,Имя,Отчество,Код_должности")] Сотрудники сотрудники)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Сотрудники");

            if (ModelState.IsValid)
            {
                _context.Add(сотрудники);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_должности"] = new SelectList(_context.Должности, "Код_должности", "Код_должности", сотрудники.Код_должности);
            return View(сотрудники);
        }

        // GET: Сотрудники/Edit/5
        [Authorize(Roles = "leader, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
          
            if (id == null || _context.Сотрудники == null)
            {
                return NotFound();
            }

            var сотрудники = await _context.Сотрудники.FindAsync(id);
            if (сотрудники == null)
            {
                return NotFound();
            }
            ViewData["Код_должности"] = new SelectList(_context.Должности, "Код_должности", "Название_должности", сотрудники.Код_должности);
            return View(сотрудники);
        }

        // POST: Сотрудники/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_сотрудника,Фамилия,Имя,Отчество,Код_должности")] Сотрудники сотрудники)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Сотрудники");

            if (id != сотрудники.Код_сотрудника)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(сотрудники);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!СотрудникиExists(сотрудники.Код_сотрудника))
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
            ViewData["Код_должности"] = new SelectList(_context.Должности, "Код_должности", "Код_должности", сотрудники.Код_должности);
            return View(сотрудники);
        }

        // GET: Сотрудники/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            

            if (id == null || _context.Сотрудники == null)
            {
                return NotFound();
            }

            var сотрудники = await _context.Сотрудники
                .Include(с => с.Должность)
                .FirstOrDefaultAsync(m => m.Код_сотрудника == id);
            if (сотрудники == null)
            {
                return NotFound();
            }

            return View(сотрудники);
        }

        // POST: Сотрудники/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Сотрудники");

            if (_context.Сотрудники == null)
            {
                return Problem("Entity set 'MyDBContext.Сотрудники'  is null.");
            }
            var сотрудники = await _context.Сотрудники.FindAsync(id);
            if (сотрудники != null)
            {
                _context.Сотрудники.Remove(сотрудники);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool СотрудникиExists(int id)
        {
          return (_context.Сотрудники?.Any(e => e.Код_сотрудника == id)).GetValueOrDefault();
        }
    }
}
