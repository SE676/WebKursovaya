using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using WebKursovaya.Data;
using WebKursovaya.Models;

public class TestController : Controller
{
    private readonly MyDBContext _context;

    // Ваш конструктор контроллера
    public TestController(MyDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }

    [HttpPost]
    public IActionResult UploadExcel(string drivingSchoolName, IFormFile file)
    {
        if (file == null || file.Length <= 0)
        {
            ModelState.AddModelError("File", "Пожалуйста, выберите файл.");
            return RedirectToAction("Error", "Test");  // Вернуть представление с сообщением об ошибке
        }

        // Получить список ожидаемых заголовков столбцов
        List<string> expectedColumnHeaders = new List<string>
    {
        "Фамилия",
        "Имя",
        "Отчество",
        "Дата сдачи",
        "Результат сдачи"
        // Добавьте остальные ожидаемые заголовки, если есть
    };

        using (var stream = new MemoryStream())
        {
            file.CopyTo(stream);
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet != null)
                {
                    if (worksheet.Dimension == null)
                    {
                        
                        ModelState.AddModelError("File", "Лист Excel пуст или не содержит данных.");
                        return RedirectToAction("Error", "Test");  // Вернуть представление с сообщением об ошибке
                    }

                    int rowCount = worksheet.Dimension.Rows;

                    // Получить фактические заголовки из первой строки файла Excel
                    List<string> actualColumnHeaders = new List<string>();
                    for (int col = 1; col <= worksheet.Dimension.Columns; col++)
                    {
                        actualColumnHeaders.Add(worksheet.Cells[1, col].Value?.ToString());
                    }

                    // Проверить соответствие заголовков
                    bool headersMatch = expectedColumnHeaders.SequenceEqual(actualColumnHeaders);
                    if (!headersMatch)
                    {
                        ModelState.AddModelError("File", "Структура столбцов не соответствует ожидаемой.");
                        return RedirectToAction("Error", "Test");  // Вернуть представление с сообщением об ошибке
                    }

                    // Продолжить обработку файла и добавление данных в базу данных
                    for (int row = 2; row <= rowCount; row++)
                    {
                        var student = new TestAS
                        {
                            Название_автошколы = drivingSchoolName,
                            Фамилия = worksheet.Cells[row, 1].Value?.ToString(),
                            Имя = worksheet.Cells[row, 2].Value?.ToString(),
                            Отчество = worksheet.Cells[row, 3].Value?.ToString(),
                            Дата_сдачи = DateTime.Parse(worksheet.Cells[row, 4].Value?.ToString()),
                            Результат_сдачи = worksheet.Cells[row, 5].Value?.ToString()
                        };

                        _context.TestAS.Add(student);
                    }

                    _context.SaveChanges();
                    return RedirectToAction("Confirmation");
                }
            }
        }

        return RedirectToAction("Index"); // Перенаправление на главную страницу или другую страницу после загрузки
    }


    //[HttpPost]
    //public IActionResult UploadExcel(string drivingSchoolName, IFormFile file)
    //{
    //    if (file == null || file.Length <= 0)
    //    {
    //        ModelState.AddModelError("File", "Пожалуйста, выберите файл.");
    //        return View(); // Вернуть представление с сообщением об ошибке
    //    }

    //    using (var stream = new MemoryStream())
    //    {
    //        file.CopyTo(stream);
    //        using (var package = new ExcelPackage(stream))
    //        {
    //            ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
    //            if (worksheet != null)
    //            {
    //                int rowCount = worksheet.Dimension.Rows;

    //                for (int row = 2; row <= rowCount; row++)
    //                {
    //                    var student = new TestAS
    //                    {
    //                        Название_автошколы = drivingSchoolName,
    //                        Фамилия = worksheet.Cells[row, 1].Value?.ToString(),
    //                        Имя = worksheet.Cells[row, 2].Value?.ToString(),
    //                        Отчество = worksheet.Cells[row, 3].Value?.ToString(),
    //                        Дата_сдачи = DateTime.Parse(worksheet.Cells[row, 4].Value?.ToString()),
    //                        Результат_сдачи = worksheet.Cells[row, 5].Value?.ToString()
    //                    };

    //                    _context.TestAS.Add(student);
    //                }

    //                _context.SaveChanges();
    //                return RedirectToAction("Confirmation");
    //            }
    //        }
    //    }

    //    return RedirectToAction("Index"); // Перенаправление на главную страницу или другую страницу после загрузки
    //}

    public IActionResult Confirmation()
    {
        // Получение последнего кода посещения из базы данных
        var последнийИндекс = _context.TestAS.Max(p => p.Код_внутреннего_экзамена);

        // Получение последней записи из базы данных
        var последняяЗапись = _context.TestAS.FirstOrDefault(p => p.Код_внутреннего_экзамена == последнийИндекс);

        var model = new TestAS(); // Создайте экземпляр вашей модели
        model.Название_автошколы = последняяЗапись.Название_автошколы; // Установите значение для свойства Название_автошколы

        return View(model); // Передача модели в представление
    }

}
