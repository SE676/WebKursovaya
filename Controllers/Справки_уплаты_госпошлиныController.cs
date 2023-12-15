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
    public class Справки_уплаты_госпошлиныController : Controller
    {
        private readonly MyDBContext _context;

        public Справки_уплаты_госпошлиныController(MyDBContext context, IAuditService auditService)
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

        // GET: Справки_уплаты_госпошлины
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Справки уплаты госпошлины");

            var myDBContext = _context.Справки_уплаты_госпошлины.Include(с => с.Дата).Include(с => с.Заявитель);
            return View(await myDBContext.ToListAsync());
        }

        // GET: Справки_уплаты_госпошлины/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Справки уплаты госпошлины");

            if (id == null || _context.Справки_уплаты_госпошлины == null)
            {
                return NotFound();
            }

            var справки_уплаты_госпошлины = await _context.Справки_уплаты_госпошлины
                .Include(с => с.Дата)
                .Include(с => с.Заявитель)
                .FirstOrDefaultAsync(m => m.Номер_справки_уплаты_госпошлины == id);
            if (справки_уплаты_госпошлины == null)
            {
                return NotFound();
            }

            return View(справки_уплаты_госпошлины);
        }

        [HttpGet]
        public async Task<IActionResult> FilterByDate(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.Справки_уплаты_госпошлины.Include(с => с.Дата).Include(с => с.Заявитель)
                    .Where(record => record.Дата.Дата >= startDate && record.Дата.Дата <= endDate)
                    .ToListAsync();

                return View(nameof(Index), filteredRecords);
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ReportByMonth(DateTime? startDate, DateTime? endDate)
        {
            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.Справки_уплаты_госпошлины.Include(с => с.Дата).Include(с => с.Заявитель)
                    .Where(record => record.Дата.Дата >= startDate && record.Дата.Дата <= endDate)
                    .ToListAsync();

                // Группировка записей по месяцам
                var monthlyCounts = filteredRecords
                    .GroupBy(record => new { record.Дата.Дата.Year, record.Дата.Дата.Month })
                    .Select(group => new { Month = new DateTime(group.Key.Year, group.Key.Month, 1), Count = group.Count() })
                    .OrderBy(item => item.Month)
                    .ToList();

                // Создание данных для графика
                var months = monthlyCounts.Select(item => item.Month.ToString("yyyy-MM"));
                var counts = monthlyCounts.Select(item => item.Count);

                ViewBag.Months = months;
                ViewBag.Counts = counts;

                return View("~/Views/Reports/ReportByMonth.cshtml"); // Представление для отображения графика
            }

            else
            {
                // Логика для формирования графика из всех записей, если даты фильтрации не указаны
                var allRecords = await _context.Справки_уплаты_госпошлины.Include(с => с.Дата).Include(с => с.Заявитель).ToListAsync();

                var monthlyCounts = allRecords
                    .GroupBy(record => new { record.Дата.Дата.Year, record.Дата.Дата.Month })
                    .Select(group => new { Month = new DateTime(group.Key.Year, group.Key.Month, 1), Count = group.Count() })
                    .OrderBy(item => item.Month)
                    .ToList();

                var months = monthlyCounts.Select(item => item.Month.ToString("yyyy-MM"));
                var counts = monthlyCounts.Select(item => item.Count);

                ViewBag.Months = months;
                ViewBag.Counts = counts;

                return View("~/Views/Reports/ReportByMonth.cshtml"); // Представление для отображения графика
            }
        }


        // GET: Справки_уплаты_госпошлины/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {
            
            ViewData["Код_стоимости"] = new SelectList(_context.Стоимость_госпошлины, "Код_стоимости", "Дата");
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО");
            return View();
        }

        // POST: Справки_уплаты_госпошлины/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Номер_справки_уплаты_госпошлины,Код_заявителя,Код_стоимости")] Справки_уплаты_госпошлины справки_уплаты_госпошлины)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Справки уплаты госпошлины");

            if (ModelState.IsValid)
            {
                _context.Add(справки_уплаты_госпошлины);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_стоимости"] = new SelectList(_context.Стоимость_госпошлины, "Код_стоимости", "Стоимость", справки_уплаты_госпошлины.Код_стоимости);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Имя", справки_уплаты_госпошлины.Код_заявителя);
            return View(справки_уплаты_госпошлины);
        }

        // GET: Справки_уплаты_госпошлины/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null || _context.Справки_уплаты_госпошлины == null)
            {
                return NotFound();
            }

            var справки_уплаты_госпошлины = await _context.Справки_уплаты_госпошлины.FindAsync(id);
            if (справки_уплаты_госпошлины == null)
            {
                return NotFound();
            }
            ViewData["Код_стоимости"] = new SelectList(_context.Стоимость_госпошлины, "Код_стоимости", "Дата", справки_уплаты_госпошлины.Код_стоимости);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО", справки_уплаты_госпошлины.Код_заявителя);
            return View(справки_уплаты_госпошлины);
        }

        // POST: Справки_уплаты_госпошлины/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Номер_справки_уплаты_госпошлины,Код_заявителя,Код_стоимости")] Справки_уплаты_госпошлины справки_уплаты_госпошлины)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Справки уплаты госпошлины");

            if (id != справки_уплаты_госпошлины.Номер_справки_уплаты_госпошлины)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(справки_уплаты_госпошлины);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Справки_уплаты_госпошлиныExists(справки_уплаты_госпошлины.Номер_справки_уплаты_госпошлины))
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
            ViewData["Код_стоимости"] = new SelectList(_context.Стоимость_госпошлины, "Код_стоимости", "Код_стоимости", справки_уплаты_госпошлины.Код_стоимости);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", справки_уплаты_госпошлины.Код_заявителя);
            return View(справки_уплаты_госпошлины);
        }

        // GET: Справки_уплаты_госпошлины/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            

            if (id == null || _context.Справки_уплаты_госпошлины == null)
            {
                return NotFound();
            }

            var справки_уплаты_госпошлины = await _context.Справки_уплаты_госпошлины
                .Include(с => с.Дата)
                .Include(с => с.Заявитель)
                .FirstOrDefaultAsync(m => m.Номер_справки_уплаты_госпошлины == id);
            if (справки_уплаты_госпошлины == null)
            {
                return NotFound();
            }

            return View(справки_уплаты_госпошлины);
        }

        // POST: Справки_уплаты_госпошлины/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Справки уплаты госпошлины");

            if (_context.Справки_уплаты_госпошлины == null)
            {
                return Problem("Entity set 'MyDBContext.Справки_уплаты_госпошлины'  is null.");
            }
            var справки_уплаты_госпошлины = await _context.Справки_уплаты_госпошлины.FindAsync(id);
            if (справки_уплаты_госпошлины != null)
            {
                _context.Справки_уплаты_госпошлины.Remove(справки_уплаты_госпошлины);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Справки_уплаты_госпошлиныExists(int id)
        {
          return (_context.Справки_уплаты_госпошлины?.Any(e => e.Номер_справки_уплаты_госпошлины == id)).GetValueOrDefault();
        }
    }
}
