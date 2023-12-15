using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebKursovaya.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Security.Cryptography;

namespace WebKursovaya.Controllers
{
    //public class AccountController : Controller
    //{
    //    private readonly IConfiguration _configuration;

    //    public AccountController(IConfiguration configuration)
    //    {
    //        _configuration = configuration;
    //    }

    //    public IActionResult Login()
    //    {
    //        return View();
    //    }

    //    [HttpPost]
    //    public async Task<IActionResult> Login(LoginViewModel model)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            string connectionString = _configuration.GetConnectionString("SecondConnection");
    //            string username = model.Username;
    //            string password = model.Password;

    //            using (SqlConnection connection = new SqlConnection(connectionString))
    //            {
    //                string query = "SELECT password_hash FROM master.sys.sql_logins WHERE name = @Username";

    //                using (SqlCommand command = new SqlCommand(query, connection))
    //                {
    //                    SqlParameter parameter = new SqlParameter("@Username", SqlDbType.NVarChar, 128);
    //                    parameter.Value = username;
    //                    command.Parameters.Add(parameter);

    //                    connection.Open();
    //                    object result = command.ExecuteScalar(); // Получение хеша пароля из базы данных

    //                    if (result != null)
    //                    {
    //                        // Преобразование байтов хеша из базы данных в строку
    //                        string hexHashedPasswordFromDB = BitConverter.ToString((byte[])result).Replace("-", "");
    //                        string trimmedHashedPassword = hexHashedPasswordFromDB.Substring(4);
    //                        //byte[] hashedBytes = StringToByteArray(hexHashedPasswordFromDB);

    //                        //// Создание объекта для вычисления хеша SHA-256

    //                        //// Вычисление хеша для массива байт
    //                        //byte[] hashResult = SHA512.Create().ComputeHash(hashedBytes);

    //                        //// Преобразование хеша в шестнадцатеричную строку
    //                        //string hashedString = BitConverter.ToString(hashResult).Replace("-", String.Empty);

    //                        // Создание экземпляра PasswordHasher
    //                        var passwordHasher = new PasswordHasher<string>();

    //                            // Хеширование пароля, введенного пользователем
    //                            string hashedPassword = BitConverter.ToString(SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes(password))).Replace("-", "");

    //                            // Сравнение хешей паролей
    //                            var resultVerification = passwordHasher.VerifyHashedPassword(null, hexHashedPasswordFromDB, hashedPassword);

    //                            if (resultVerification == PasswordVerificationResult.Success)
    //                            {
    //                                // Успешная аутентификация пользователя
    //                                // Вы можете выполнить вход пользователя или перенаправить на другую страницу
    //                                return RedirectToAction("Index", "Home");
    //                            }
    //                        }
    //                    }
    //                }

    //                // Неверные учетные данные
    //                ModelState.AddModelError(string.Empty, "Неверное имя пользователя или пароль");
    //                return View(model);
    //            }

    //            //Некорректная модель - отображаем форму входа снова
    //            return View(model);
    //        }

    //        static byte[] StringToByteArray(string hex)
    //        {
    //            int numberChars = hex.Length;
    //            byte[] bytes = new byte[numberChars / 2];
    //            for (int i = 0; i < numberChars; i += 2)
    //            {
    //                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
    //            }
    //            return bytes;
    //        }

    //    }
    }