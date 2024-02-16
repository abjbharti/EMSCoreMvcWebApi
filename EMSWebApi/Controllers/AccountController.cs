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
        public async Task<IActionResult> SignUp(Register register)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.Signup(register);

                if (result != null)
                {
                    if (result.IsSuccess)
                    {
                        return Ok(new { Message = "Registration successful" });
                    }
                    return BadRequest(new { Message = "Registration failed" });
                }
            }
            return BadRequest(new { Message = "Invalid model state" });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(Login login)
        {
            var result = await _accountRepository.Login(login);

            if (result != null)
            {
                if (result.IsSuccess)
                {
                    return Ok(new
                    {
                        result.Token,
                        result.Username,
                        result.Role
                    });
                }

                return Unauthorized(new { Message = "Authentication failed" });
            }
            return Unauthorized(new { Message = "Invalid login credentials" });
        }
    }
}
