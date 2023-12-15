using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebKursovaya.Data;
using WebKursovaya.Models;

namespace WebKursovaya.Controllers
{
    public class QualificationController : Controller
    {
        private readonly MyDBContext _context;
        public IActionResult Index()
        {
            return View();
        }

        public QualificationController(MyDBContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            var viewModel = new QualificationViewModel();

            ViewBag.Applicants = new SelectList(_context.Заявители.ToList(), "Код_заявителя", "ЗаявительФИО");

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(QualificationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                using var transaction = _context.Database.BeginTransaction();

                try
                {
                    // Проверка, был ли выбран существующий заявитель или нужно создать нового
                    if (viewModel.Документы_о_квалификации.Код_заявителя == 0) // Значение по умолчанию
                    {
                        var newApplicant = new Заявители
                        {
                            Фамилия = viewModel.Заявитель.Фамилия,
                            Имя = viewModel.Заявитель.Имя,
                            Отчество = viewModel.Заявитель.Отчество,
                            Дата_рождения = viewModel.Заявитель.Дата_рождения,
                            // Другие свойства заявителя
                        };

                        _context.Заявители.Add(newApplicant);
                        await _context.SaveChangesAsync();

                        viewModel.Документы_о_квалификации.Код_заявителя = newApplicant.Код_заявителя;
                    }

                    _context.Документы_о_Квалификации.Add(viewModel.Документы_о_квалификации);
                    await _context.SaveChangesAsync();

                    transaction.Commit();

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    ModelState.AddModelError(string.Empty, "Ошибка при создании записи");
                    // Обработка ошибки
                }
            }

            ViewBag.Applicants = new SelectList(_context.Заявители.ToList(), "Код_заявителя", "ЗаявительФИО");
            return View(viewModel);
        }


    }
}
