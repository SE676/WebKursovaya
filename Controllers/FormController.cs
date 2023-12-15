using Microsoft.AspNetCore.Mvc;
using WebKursovaya.Data;
using WebKursovaya.Models;

namespace WebKursovaya.Controllers
{
    public class FormController : Controller
    {
        private readonly MyDBContext _context;

        public FormController(MyDBContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Посещения посещение)
        {
            // Проверка наличия записи с такими же данными
            var записьВБазе = _context.Посещения.FirstOrDefault(p =>
                p.Фамилия == посещение.Фамилия &&
                p.Имя == посещение.Имя &&
                p.Отчество == посещение.Отчество &&
                p.Дата_рождения == посещение.Дата_рождения &&
                p.Дата_посещения == посещение.Дата_посещения);

            if (записьВБазе != null)
            {
                // Запись уже существует, возвращаем ошибку
                ModelState.AddModelError(string.Empty, "Данные уже существуют в базе");
            }

            // Проверка занятости выбранной даты
            var записьСВыбраннойДатой = _context.Посещения.FirstOrDefault(p =>
                p.Дата_посещения == посещение.Дата_посещения);

            if (записьСВыбраннойДатой != null)
            {
                // Дата уже занята, возвращаем ошибку
                ModelState.AddModelError("Дата_посещения", "Выбранная дата уже занята");
            }

            if (ModelState.IsValid)
            {
                // Все остальные действия сохранения в базу данных
                _context.Посещения.Add(посещение);
                _context.SaveChanges();
                return RedirectToAction("Confirmation");
            }

            return View(посещение);
        }

        public IActionResult Confirmation()
        {
            // Получение последнего кода посещения из базы данных
            var последнийИндекс = _context.Посещения.Max(p => p.Код_посещения);
            
            // Получение последней записи из базы данных
            var последняяЗапись = _context.Посещения.FirstOrDefault(p => p.Код_посещения == последнийИндекс);

            // Получение выбранной даты, если последняя запись существует
            DateTime выбраннаяДата = последняяЗапись != null ? последняяЗапись.Дата_посещения : DateTime.Now; // DateTime.Now - просто для примера, может быть другой дефолтной датой


            // Создание модели, в которой передается выбранная дата
            var model = new Посещения { Дата_посещения = выбраннаяДата };

            return View(model);
        }
    }
}

