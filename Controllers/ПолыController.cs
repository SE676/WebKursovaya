using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml;
using WebKursovaya.Data;
using WebKursovaya.Models;
using System.IO;

namespace WebKursovaya.Controllers
{
    public class ПолыController : Controller
    {
        private readonly MyDBContext _context;
        private readonly SecondDBContext _context2;
        public ПолыController(MyDBContext context, SecondDBContext context2, IAuditService auditService)
        {
            _context = context;
            _context2 = context2;
            _auditService = auditService;
        }

        private readonly IAuditService _auditService;

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied"); // Отображение страницы с сообщением об отказе в доступе
        }


        // GET: Полы
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            ViewBag.StartDate = TempData["StartDate"];
            ViewBag.EndDate = TempData["EndDate"];

            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Полы");


            return _context.Полы != null ?
                          View(await _context.Полы.ToListAsync()) :
                          Problem("Entity set 'MyDBContext.Полы'  is null.");


        }

        // GET: Полы/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Полы");

            if (id == null || _context.Полы == null)
            {
                return NotFound();
            }

            var полы = await _context.Полы
                .FirstOrDefaultAsync(m => m.Код_пола == id);
            if (полы == null)
            {
                return NotFound();
            }

            return View(полы);
        }

        // GET: Полы/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Полы/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_пола,Пол")] Полы полы)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Полы");

            if (ModelState.IsValid)
            {
                //полы.Дата_создания_записи = DateTime.Now;
                _context.Add(полы);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(полы);
        }

        // GET: Полы/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            

            if (id == null || _context.Полы == null)
            {
                return NotFound();
            }

            var полы = await _context.Полы.FindAsync(id);
            if (полы == null)
            {
                return NotFound();
            }
            return View(полы);
        }


        // POST: Полы/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_пола,Пол")] Полы полы)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Полы");

            if (id != полы.Код_пола)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(полы);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ПолыExists(полы.Код_пола))
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
            return View(полы);
        }

        // GET: Полы/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null || _context.Полы == null)
            {
                return NotFound();
            }

            var полы = await _context.Полы
                .FirstOrDefaultAsync(m => m.Код_пола == id);
            if (полы == null)
            {
                return NotFound();
            }

            return View(полы);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Полы");

            var полы = await _context.Полы.FindAsync(id);

            if (полы == null)
            {
                return NotFound();
            }

            // Создание объекта архивной записи
            var Полы_архив = new Полы
            {
                Код_пола = полы.Код_пола,
                Пол = полы.Пол
            };

            // Добавление записи в таблицу архива
            _context2.Полы.Add(Полы_архив);

            // Удаление из основной таблицы
            _context.Полы.Remove(полы);

            await _context2.SaveChangesAsync();
            await _context.SaveChangesAsync();
            

            return RedirectToAction(nameof(Index));
        }


        private bool ПолыExists(int id)
        {
          return (_context.Полы?.Any(e => e.Код_пола == id)).GetValueOrDefault();
        }
    }
}
