using EMSModels;
using EMSWebApi.AccountRepo;
using Microsoft.AspNetCore.Mvc;

namespace EMSWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        [HttpPost("signup")]
        public IActionResult SignUp(Register register)
        {
            if (ModelState.IsValid)
            {
                var result = _accountRepository.Signup(register);

                if (result.Result != null)
                {
                    if (result.Result.IsSuccess)
                    {
                        return Ok(new { Message = "Registration successful" });
                    }
                    return BadRequest(new { Message = "Registration failed" });
                }
            }
            return BadRequest(new { Message = "Invalid model state" });
        }


        [HttpPost("login")]
        public IActionResult Login(Login login)
        {
            var result = _accountRepository.Login(login);

            if (result != null)
            {
                if (result.Result.IsSuccess)
                {
                    return Ok(new
                    {
                        result.Result.Token,
                        result.Result.Username,
                        result.Result.Role
                    });
                }

                return Unauthorized(new { Message = "Authentication failed" });
            }
            return Unauthorized(new { Message = "Invalid login credentials" });
        }
    }
}
