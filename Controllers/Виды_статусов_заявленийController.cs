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
    public class Виды_статусов_заявленийController : Controller
    {

        private readonly MyDBContext _context;

        public Виды_статусов_заявленийController(MyDBContext context, IAuditService auditService)
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


        // GET: Виды_статусов_заявлений
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Статусы заявлений");

            return _context.Виды_статусов_заявлений != null ? 
                          View(await _context.Виды_статусов_заявлений.ToListAsync()) :
                          Problem("Entity set 'MyDBContext.Виды_статусов_заявлений'  is null.");
        }

        // GET: Виды_статусов_заявлений/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {

            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Статусы заявлений");

            if (id == null || _context.Виды_статусов_заявлений == null)
            {
                return NotFound();
            }

            var виды_статусов_заявлений = await _context.Виды_статусов_заявлений
                .FirstOrDefaultAsync(m => m.Код_статуса_заявления == id);
            if (виды_статусов_заявлений == null)
            {
                return NotFound();
            }

            return View(виды_статусов_заявлений);
        }

        // GET: Виды_статусов_заявлений/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Виды_статусов_заявлений/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_статуса_заявления,Название_статуса")] Виды_статусов_заявлений виды_статусов_заявлений)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Статусы заявлений");

            if (ModelState.IsValid)
            {
                _context.Add(виды_статусов_заявлений);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(виды_статусов_заявлений);
        }

        // GET: Виды_статусов_заявлений/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
           

            if (id == null || _context.Виды_статусов_заявлений == null)
            {
                return NotFound();
            }

            var виды_статусов_заявлений = await _context.Виды_статусов_заявлений.FindAsync(id);
            if (виды_статусов_заявлений == null)
            {
                return NotFound();
            }
            return View(виды_статусов_заявлений);
        }

        // POST: Виды_статусов_заявлений/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_статуса_заявления,Название_статуса")] Виды_статусов_заявлений виды_статусов_заявлений)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Статусы заявлений");


            if (id != виды_статусов_заявлений.Код_статуса_заявления)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(виды_статусов_заявлений);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Виды_статусов_заявленийExists(виды_статусов_заявлений.Код_статуса_заявления))
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
            return View(виды_статусов_заявлений);
        }

        // GET: Виды_статусов_заявлений/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {

         
            if (id == null || _context.Виды_статусов_заявлений == null)
            {
                return NotFound();
            }

            var виды_статусов_заявлений = await _context.Виды_статусов_заявлений
                .FirstOrDefaultAsync(m => m.Код_статуса_заявления == id);
            if (виды_статусов_заявлений == null)
            {
                return NotFound();
            }

            return View(виды_статусов_заявлений);
        }

        // POST: Виды_статусов_заявлений/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Статусы заявлений");

            if (_context.Виды_статусов_заявлений == null)
            {
                return Problem("Entity set 'MyDBContext.Виды_статусов_заявлений'  is null.");
            }
            var виды_статусов_заявлений = await _context.Виды_статусов_заявлений.FindAsync(id);
            if (виды_статусов_заявлений != null)
            {
                _context.Виды_статусов_заявлений.Remove(виды_статусов_заявлений);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Виды_статусов_заявленийExists(int id)
        {
          return (_context.Виды_статусов_заявлений?.Any(e => e.Код_статуса_заявления == id)).GetValueOrDefault();
        }
    }
}
