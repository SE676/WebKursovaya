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
    public class ВУController : Controller
    {
        private readonly MyDBContext _context;

        public ВУController(MyDBContext context, IAuditService auditService)
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

        // GET: ВУ
        [Authorize(Roles = "staff_member, leader, fd, examiner, admin")]
        public async Task<IActionResult> Index()
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ВУ");

            var myDBContext = _context.ВУ.Include(в => в.Виды_статусов_ВУ).Include(в => в.Заявитель);
            return View(await myDBContext.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> FilterByDate(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.ВУ.Include(в => в.Виды_статусов_ВУ).Include(в => в.Заявитель)
                    .Where(record => record.Дата_выдачи >= startDate && record.Дата_выдачи <= endDate)
                    .ToListAsync();

                return View(nameof(Index), filteredRecords);
            }

            return RedirectToAction(nameof(Index));
        }


        //public async Task<IActionResult> ReportByMonth(DateTime? startDate, DateTime? endDate)
        //{
        //    if (startDate != null && endDate != null)
        //    {
        //        var filteredRecords = await _context.ВУ
        //            .Where(record => record.Дата_выдачи >= startDate && record.Дата_выдачи <= endDate)
        //            .ToListAsync();

        //        // Группировка записей по месяцам
        //        var monthlyCounts = filteredRecords
        //            .GroupBy(record => new { record.Дата_выдачи.Year, record.Дата_выдачи.Month })
        //            .Select(group => new { Month = new DateTime(group.Key.Year, group.Key.Month, 1), Count = group.Count() })
        //            .OrderBy(item => item.Month)
        //            .ToList();

        //        // Создание данных для графика
        //        var months = monthlyCounts.Select(item => item.Month.ToString("yyyy-MM"));
        //        var counts = monthlyCounts.Select(item => item.Count);

        //        ViewBag.Months = months;
        //        ViewBag.Counts = counts;

        //        return View("~/Views/Reports/ReportByMonth.cshtml"); // Представление для отображения графика
        //    }

        //    else
        //    {
        //        // Логика для формирования графика из всех записей, если даты фильтрации не указаны
        //        var allRecords = await _context.ВУ.ToListAsync();

        //        var monthlyCounts = allRecords
        //            .GroupBy(record => new { record.Дата_выдачи.Year, record.Дата_выдачи.Month })
        //            .Select(group => new { Month = new DateTime(group.Key.Year, group.Key.Month, 1), Count = group.Count() })
        //            .OrderBy(item => item.Month)
        //            .ToList();

        //        var months = monthlyCounts.Select(item => item.Month.ToString("yyyy-MM"));
        //        var counts = monthlyCounts.Select(item => item.Count);

        //        ViewBag.Months = months;
        //        ViewBag.Counts = counts;

        //        return View("~/Views/Reports/ReportByMonth.cshtml"); // Представление для отображения графика
        //    }
        //}

        public async Task<IActionResult> ReportByVUStatus(DateTime? startDate, DateTime? endDate)
        {
            if (startDate != null && endDate != null)
            {
                var filteredRecords = await _context.ВУ.Include(vu => vu.Виды_статусов_ВУ)
                            .Where(record => record.Дата_выдачи >= startDate && record.Дата_выдачи <= endDate)
                            .ToListAsync();
                var vuStatusCounts = filteredRecords
                    .GroupBy(vu => vu.Виды_статусов_ВУ?.Название_статуса ?? "Неизвестный статус")
                    .Select(group => new
                    {
                        Status = group.Key,
                        Count = group.Count(),
                        Color = GetColorForStatus(group.Key), // Получаем цвет для каждого статуса
                        Label = group.Key // Используем статус ВУ в качестве меки
                    })
                    .ToList();

                // Создаем массивы данных для представления
                var vuStatuses = vuStatusCounts.Select(item => item.Status);
                var vuCounts = vuStatusCounts.Select(item => item.Count);
                var vuColors = vuStatusCounts.Select(item => item.Color);
                var vuLabels = vuStatusCounts.Select(item => item.Label); // Создаем массив меток

                // Передаем данные в ViewBag
                ViewBag.VUStatuses = vuStatuses;
                ViewBag.VUCounts = vuCounts;
                ViewBag.VUColors = vuColors;
                ViewBag.VULabels = vuLabels; // Передаем массив меток

                return View("~/Views/Reports/ReportVU.cshtml"); // Представление для отображения отчета по статусам ВУ
            }

            else
            {
                var allVUs = await _context.ВУ.Include(vu => vu.Виды_статусов_ВУ).ToListAsync();

                // Получение списка статусов ВУ и подсчет количества для каждого статуса
                var vuStatusCounts = allVUs
                    .GroupBy(vu => vu.Виды_статусов_ВУ?.Название_статуса ?? "Неизвестный статус")
                    .Select(group => new
                    {
                        Status = group.Key,
                        Count = group.Count(),
                        Color = GetColorForStatus(group.Key), // Получаем цвет для каждого статуса
                        Label = group.Key // Используем статус ВУ в качестве метки
                    })
                    .ToList();

                // Создаем массивы данных для представления
                var vuStatuses = vuStatusCounts.Select(item => item.Status);
                var vuCounts = vuStatusCounts.Select(item => item.Count);
                var vuColors = vuStatusCounts.Select(item => item.Color);
                var vuLabels = vuStatusCounts.Select(item => item.Label); // Создаем массив меток

                // Передаем данные в ViewBag
                ViewBag.VUStatuses = vuStatuses;
                ViewBag.VUCounts = vuCounts;
                ViewBag.VUColors = vuColors;
                ViewBag.VULabels = vuLabels; // Передаем массив меток

                return View("~/Views/Reports/ReportVU.cshtml"); // Представление для отображения отчета по статусам ВУ
            }

        }
        private string GetColorForStatus(string status)
        {
            switch (status)
            {
                case "действителен":
                    return "green"; // Зеленый цвет для статуса "действителен"
                case "истек срок действия":
                    return "yellow"; // Желтый цвет для статуса "истек срок действия"
                case "лишен":
                    return "red"; // Желтый цвет для статуса "истек срок действия"
                default:
                    return "grey"; // Серый цвет для остальных статусов
            }
        }


        // GET: ВУ/Details/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Details(int? id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "SELECT", "ВУ");


            if (id == null || _context.ВУ == null)
            {
                return NotFound();
            }

            var вУ = await _context.ВУ
                .Include(в => в.Виды_статусов_ВУ)
                .Include(в => в.Заявитель)
                .FirstOrDefaultAsync(m => m.Код_ВУ == id);
            if (вУ == null)
            {
                return NotFound();
            }

            return View(вУ);
        }

        // GET: ВУ/Create
        [Authorize(Roles = "staff_member, admin")]
        public IActionResult Create()
        {

            ViewData["Код_статуса_ВУ"] = new SelectList(_context.Виды_статусов_ВУ, "Код_статуса_ВУ", "Название_статуса");
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО");
            return View();
        }

        // POST: ВУ/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Код_ВУ,Код_статуса_ВУ,Код_заявителя,Серия_ВУ,Номер_ВУ,Дата_выдачи,Дата_окончания_срока_действия")] ВУ вУ)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "INSERT", "ВУ");

            if (ModelState.IsValid)
            {
                вУ.Дата_выдачи = DateTime.Now;
                вУ.Дата_окончания_срока_действия = вУ.Дата_выдачи.AddYears(10);
                _context.Add(вУ);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Код_статуса_ВУ"] = new SelectList(_context.Виды_статусов_ВУ, "Код_статуса_ВУ", "Код_статуса_ВУ", вУ.Код_статуса_ВУ);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", вУ.Код_заявителя);
            return View(вУ);
        }

        // GET: ВУ/Edit/5
        [Authorize(Roles = "staff_member, admin")]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null || _context.ВУ == null)
            {
                return NotFound();
            }

            var вУ = await _context.ВУ.FindAsync(id);
            if (вУ == null)
            {
                return NotFound();
            }
            ViewData["Код_статуса_ВУ"] = new SelectList(_context.Виды_статусов_ВУ, "Код_статуса_ВУ", "Название_статуса", вУ.Код_статуса_ВУ);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "ЗаявительФИО", вУ.Код_заявителя);
            return View(вУ);
        }

        // POST: ВУ/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Код_ВУ,Код_статуса_ВУ,Код_заявителя,Серия_ВУ,Номер_ВУ,Дата_выдачи,Дата_окончания_срока_действия")] ВУ вУ)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "UPDATE", "ВУ");

            if (id != вУ.Код_ВУ)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(вУ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ВУExists(вУ.Код_ВУ))
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
            ViewData["Код_статуса_ВУ"] = new SelectList(_context.Виды_статусов_ВУ, "Код_статуса_ВУ", "Код_статуса_ВУ", вУ.Код_статуса_ВУ);
            ViewData["Код_заявителя"] = new SelectList(_context.Заявители, "Код_заявителя", "Код_заявителя", вУ.Код_заявителя);
            return View(вУ);
        }

        // GET: ВУ/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || _context.ВУ == null)
            {
                return NotFound();
            }

            var вУ = await _context.ВУ
                .Include(в => в.Виды_статусов_ВУ)
                .Include(в => в.Заявитель)
                .FirstOrDefaultAsync(m => m.Код_ВУ == id);
            if (вУ == null)
            {
                return NotFound();
            }

            return View(вУ);
        }

        // POST: ВУ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string userName = User.Identity.Name;
            string role = User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;
            _auditService.LogAction(userName, role, "DELETE", "ВУ");

            if (_context.ВУ == null)
            {
                return Problem("Entity set 'MyDBContext.ВУ'  is null.");
            }
            var вУ = await _context.ВУ.FindAsync(id);
            if (вУ != null)
            {
                _context.ВУ.Remove(вУ);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ВУExists(int id)
        {
            return (_context.ВУ?.Any(e => e.Код_ВУ == id)).GetValueOrDefault();
        }
    }
}
