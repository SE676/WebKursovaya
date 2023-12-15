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
    public class ЭкзаменыController : Controller
    {
        private readonly MyDBContext _context;

        public ЭкзаменыController(MyDBContext context, IAuditService auditService)
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

        // GET: Экзамены
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Экзамены");

            var myDBContext = _context.Экзамены.Include(э => э.Заявитель).Include(э => э.Категория).Include(э => э.Сотрудник);
            return View(await myDBContext.ToListAsync());
        }


        [HttpGet]
        public async Task<IActionResult> FilterByDate(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.Экзамены.Include(э => э.Заявитель).Include(э => э.Категория).Include(э => э.Сотрудник)
                    .Where(record => record.Дата_сдачи >= startDate && record.Дата_сдачи <= endDate).ToListAsync();

                return View(nameof(Index), filteredRecords);
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> ReportByMonth(DateTime? startDate, DateTime? endDate)
        {
            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.Экзамены
                    .Where(record => record.Дата_сдачи >= startDate && record.Дата_сдачи <= endDate)
                    .ToListAsync();

                // Группировка записей по месяцам и результату сдачи
                var monthlyCounts = filteredRecords
                    .GroupBy(record => new DateTime(record.Дата_сдачи.Year, record.Дата_сдачи.Month, 1))
                    .Select(group => new
                    {
                        Month = group.Key,
                        Results = group.GroupBy(r => r.Результат_сдачи)
                                      .ToDictionary(gr => gr.Key, gr => gr.Count())
                    })
                    .OrderBy(item => item.Month)
                    .ToList();

                // Создание данных для графика
                var months = monthlyCounts.Select(item => item.Month.ToString("yyyy-MM"));
                var passedCounts = monthlyCounts.Select(item => item.Results.ContainsKey("сдан") ? item.Results["сдан"] : 0);
                var failedCounts = monthlyCounts.Select(item => item.Results.ContainsKey("не сдан") ? item.Results["не сдан"] : 0);
                var passedColors = monthlyCounts.SelectMany(item => item.Results.ContainsKey("сдан") && item.Results["сдан"] > 0
    ? Enumerable.Repeat("green", item.Results["сдан"])
    : Enumerable.Repeat("transparent", 1));

                var failedColors = monthlyCounts.SelectMany(item => item.Results.ContainsKey("не сдан") && item.Results["не сдан"] > 0
                    ? Enumerable.Repeat("red", item.Results["не сдан"])
                    : Enumerable.Repeat("transparent", 1));



                ViewBag.Months = months;
                ViewBag.PassedCounts = passedCounts;
                ViewBag.FailedCounts = failedCounts;
                ViewBag.PassedColors = passedColors;
                ViewBag.FailedColors = failedColors;

                return View("~/Views/Reports/ReportExams.cshtml"); // Представление для отображения графика
            }

            else
            {
                var allRecords = await _context.Экзамены.ToListAsync();
                // Группировка всех записей по месяцам
                var monthlyRecords = allRecords
                    .GroupBy(record => new DateTime(record.Дата_сдачи.Year, record.Дата_сдачи.Month, 1))
                    .Select(group => new
                    {
                        Month = group.Key,
                        Results = group.GroupBy(r => r.Результат_сдачи)
                                      .ToDictionary(gr => gr.Key, gr => gr.Count())
                    })
                    .OrderBy(item => item.Month)
                    .ToList();

                // Создание данных для графика
                var months = monthlyRecords.Select(item => item.Month.ToString("yyyy-MM"));
                var passedCounts = monthlyRecords.Select(item => item.Results.ContainsKey("сдан") ? item.Results["сдан"] : 0);
                var failedCounts = monthlyRecords.Select(item => item.Results.ContainsKey("не сдан") ? item.Results["не сдан"] : 0);
                var passedColors = monthlyRecords.SelectMany(item => item.Results.ContainsKey("сдан") && item.Results["сдан"] > 0
    ? Enumerable.Repeat("green", item.Results["сдан"])
    : Enumerable.Repeat("transparent", 1));

                var failedColors = monthlyRecords.SelectMany(item => item.Results.ContainsKey("не сдан") && item.Results["не сдан"] > 0
                    ? Enumerable.Repeat("red", item.Results["не сдан"])
                    : Enumerable.Repeat("transparent", 1));
                ViewBag.Months = months;
                ViewBag.PassedCounts = passedCounts;
                ViewBag.FailedCounts = failedCounts;
                ViewBag.PassedColors = passedColors;
                ViewBag.FailedColors = failedColors;
                return View("~/Views/Reports/ReportExams.cshtml"); // Представление для отображения графика

            }
        }



        // GET: Экзамены/Details/5
        [Authorize(Roles = "examiner, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "Экзамены");

            if (id == null || _context.Экзамены == null)
            {
                return NotFound();
            }

            var экзамены = await _context.Экзамены
                .Include(э => э.Заявитель)
                .Include(э => э.Категория)
                .Include(э => э.Сотрудник)
                .FirstOrDefaultAsync(m => m.Код_экзамена == id);
            if (экзамены == null)
            {
                return NotFound();
            }

            return View(экзамены);
        }

        // GET: Экзамены/Create
        [Authorize(Roles = "examiner, admin")]
        public IActionResult Create()
        {
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО");
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС");
            ViewData["Код_сотрудника"] = new SelectList(_context.Сотрудники, "Код_сотрудника", "СотрудникФИО");
            return View();
        }

        // POST: Экзамены/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_экзамена,Код_заявителя,Дата_сдачи,Код_сотрудника,Код_категории_ТС,Результат_сдачи")] Экзамены экзамены)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "Экзамены");

            if (ModelState.IsValid)
            {
                экзамены.Дата_сдачи = DateTime.Now;
                _context.Add(экзамены);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", экзамены.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Код_категории_ТС", экзамены.Код_категории_ТС);
            ViewData["Код_сотрудника"] = new SelectList(_context.Сотрудники, "Код_сотрудника", "Код_сотрудника", экзамены.Код_сотрудника);
            return View(экзамены);
        }

        // GET: Экзамены/Edit/5
        [Authorize(Roles = "examiner, admin")]
        public async Task<IActionResult> Edit(int? id)
        {
          
            if (id == null || _context.Экзамены == null)
            {
                return NotFound();
            }

            var экзамены = await _context.Экзамены.FindAsync(id);
            if (экзамены == null)
            {
                return NotFound();
            }
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО", экзамены.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Наименование_категории_ТС", экзамены.Код_категории_ТС);
            ViewData["Код_сотрудника"] = new SelectList(_context.Сотрудники, "Код_сотрудника", "СотрудникФИО", экзамены.Код_сотрудника);
            return View(экзамены);
        }

        // POST: Экзамены/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_экзамена,Код_заявителя,Дата_сдачи,Код_сотрудника,Код_категории_ТС,Результат_сдачи")] Экзамены экзамены)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "Экзамены");

            if (id != экзамены.Код_экзамена)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(экзамены);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ЭкзаменыExists(экзамены.Код_экзамена))
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
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", экзамены.Код_заявителя);
            ViewData["Код_категории_ТС"] = new SelectList(_context.Категории_ТС, "Код_категории_ТС", "Код_категории_ТС", экзамены.Код_категории_ТС);
            ViewData["Код_сотрудника"] = new SelectList(_context.Сотрудники, "Код_сотрудника", "Код_сотрудника", экзамены.Код_сотрудника);
            return View(экзамены);
        }

        // GET: Экзамены/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
           

            if (id == null || _context.Экзамены == null)
            {
                return NotFound();
            }

            var экзамены = await _context.Экзамены
                .Include(э => э.Заявитель)
                .Include(э => э.Категория)
                .Include(э => э.Сотрудник)
                .FirstOrDefaultAsync(m => m.Код_экзамена == id);
            if (экзамены == null)
            {
                return NotFound();
            }

            return View(экзамены);
        }

        // POST: Экзамены/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "Экзамены");

            if (_context.Экзамены == null)
            {
                return Problem("Entity set 'MyDBContext.Экзамены'  is null.");
            }
            var экзамены = await _context.Экзамены.FindAsync(id);
            if (экзамены != null)
            {
                _context.Экзамены.Remove(экзамены);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ЭкзаменыExists(int id)
        {
            return (_context.Экзамены?.Any(e => e.Код_экзамена == id)).GetValueOrDefault();
        }
    }
}
