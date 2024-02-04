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
    public class ЗаявленияController : Controller
    {
        private readonly MyDBContext _context;

        public ЗаявленияController(MyDBContext context, IAuditService auditService)
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


        // GET: Заявления
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Заявления");

            var myDBContext = _context.Заявления.Include(з => з.Вид_услуги).Include(з => з.Виды_статусов_заявлений).Include(з => з.Заявитель);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Заявления/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Заявления");

            if (id == null || _context.Заявления == null)
            {
                return NotFound();
            }

            var заявления = await _context.Заявления
                .Include(з => з.Вид_услуги)
                .Include(з => з.Виды_статусов_заявлений)
                .Include(з => з.Заявитель)
                .FirstOrDefaultAsync(m => m.Номер_заявления == id);
            if (заявления == null)
            {
                return NotFound();
            }

            return View(заявления);
        }

        // GET: Заявления/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
        
            ViewData["Код_вида_услуги"] = new SelectList(_context.Виды_услуг, "Код_вида_услуги", "Название_вида_услуги");
            ViewData["Код_статуса_заявления"] = new SelectList(_context.Виды_статусов_заявлений, "Код_статуса_заявления", "Название_статуса");
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО");
            return View();
        }

        // POST: Заявления/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Номер_заявления,Код_статуса_заявления,Код_заявителя,Код_вида_услуги")] Заявления заявления)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Заявления");


            if (ModelState.IsValid)
            {
                _context.Add(заявления);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_вида_услуги"] = new SelectList(_context.Виды_услуг, "Код_вида_услуги", "Код_вида_услуги", заявления.Код_вида_услуги);
            ViewData["Код_статуса_заявления"] = new SelectList(_context.Виды_статусов_заявлений, "Код_статуса_заявления", "Код_статуса_заявления", заявления.Код_статуса_заявления);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", заявления.Код_заявителя);
            return View(заявления);
        }

        // GET: Заявления/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Заявления == null)
            {
                return NotFound();
            }

            var заявления = await _context.Заявления.FindAsync(id);
            if (заявления == null)
            {
                return NotFound();
            }
            ViewData["Код_вида_услуги"] = new SelectList(_context.Виды_услуг, "Код_вида_услуги", "Название_вида_услуги", заявления.Код_вида_услуги);
            ViewData["Код_статуса_заявления"] = new SelectList(_context.Виды_статусов_заявлений, "Код_статуса_заявления", "Название_статуса", заявления.Код_статуса_заявления);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО", заявления.Код_заявителя);
            return View(заявления);
        }

        // POST: Заявления/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Номер_заявления,Код_статуса_заявления,Код_заявителя,Код_вида_услуги")] Заявления заявления)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Заявления");

            if (id != заявления.Номер_заявления)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(заявления);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ЗаявленияExists(заявления.Номер_заявления))
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
            ViewData["Код_вида_услуги"] = new SelectList(_context.Виды_услуг, "Код_вида_услуги", "Код_вида_услуги", заявления.Код_вида_услуги);
            ViewData["Код_статуса_заявления"] = new SelectList(_context.Виды_статусов_заявлений, "Код_статуса_заявления", "Код_статуса_заявления", заявления.Код_статуса_заявления);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", заявления.Код_заявителя);
            return View(заявления);
        }


        // GET: Заявления/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
           

            if (id == null || _context.Заявления == null)
            {
                return NotFound();
            }

            var заявления = await _context.Заявления
                .Include(з => з.Вид_услуги)
                .Include(з => з.Виды_статусов_заявлений)
                .Include(з => з.Заявитель)
                .FirstOrDefaultAsync(m => m.Номер_заявления == id);
            if (заявления == null)
            {
                return NotFound();
            }

            return View(заявления);
        }

        // POST: Заявления/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Заявления");

            if (_context.Заявления == null)
            {
                return Problem("Entity set 'MyDBContext.Заявления'  is null.");
            }
            var заявления = await _context.Заявления.FindAsync(id);
            if (заявления != null)
            {
                _context.Заявления.Remove(заявления);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ЗаявленияExists(int id)
        {
          return (_context.Заявления?.Any(e => e.Номер_заявления == id)).GetValueOrDefault();
        }
    }
}
