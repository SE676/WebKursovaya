using Microsoft.AspNetCore.Mvc;
using WebKursovaya.ViewModels;

namespace WebKursovaya.Controllers
{


    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly SqlLoginsService _sqlLoginsService;

        public AuthController(SqlLoginsService sqlLoginsService)
        {
            _sqlLoginsService = sqlLoginsService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                bool isValid = _sqlLoginsService.ValidateLogin(model.Username, model.Password);

                if (isValid)
                {
                    // Аутентификация прошла успешно
                    // Здесь можно создать и вернуть токен аутентификации или выполнить другие действия
                    return Ok("Authentication successful");
                }
            }

            // Если аутентификация не удалась
            return Unauthorized("Invalid username or password");
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


}
