using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreAPIMySQL.Data.Repositories;
using NetCoreAPIMySQL.Model;
using NetCoreAPIMySQL.Model.Requests;
using NetCoreAPIMySQL.Model.Responses;

namespace WebTestNET.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserRepository _userService;

        public UsersController(IUserRepository userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return Ok(new { status = 0, message = "Nombre de usuario o contraseña incorrecta" });

            return Ok(new { status = 1, response = response });
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(new { status = 1, response = users });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult<RegisterResponse> Register(RegisterRequest registerRequest)
        {
            var responseRegister = _userService.Register(registerRequest);

            if (responseRegister == null)
                return Ok(new { status = responseRegister.status, message = "Debes llenar todos los campos" });

            return Ok(new { status = responseRegister.status, message = responseRegister.response });
        }

    }
}

