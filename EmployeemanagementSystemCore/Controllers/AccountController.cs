using EMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EmployeemanagementSystemCore.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Login login)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7098/");
                var postTask = await client.PostAsJsonAsync<Login>("api/Account/Login", login);
                if (postTask.IsSuccessStatusCode)
                {
                    var customerJsonString = postTask.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginModel>(custome‌​rJsonString.Result);

                    HttpContext.Session.SetString("Token", deserialized.Token);
                    HttpContext.Session.SetString("Username", deserialized.Username);

                    HttpContext.Session.SetString("UserRole", deserialized.Role);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, deserialized.Username),
                        new Claim(ClaimTypes.Role, deserialized.Role)
                    };

                    var identity = new ClaimsIdentity(claims, "Custom");
                    HttpContext.User = new ClaimsPrincipal(identity);

                    if (deserialized.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manager");
                    }
                }
                ModelState.AddModelError(string.Empty, "Please enter the valid email and password.");
            }

            return View(login);
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Register register)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7098/");

                var postTask = client.PostAsJsonAsync<Register>("api/Account/SignUp", register);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = result.RequestMessage;
                    return RedirectToAction("Login");
                }
            }

            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");

            return View(register);
        }

        public ActionResult Logout()
        {
            using (var client = new HttpClient())
            {
                HttpContext.Session.Clear();

                return RedirectToAction("Login");
            }
        }
    }
}
