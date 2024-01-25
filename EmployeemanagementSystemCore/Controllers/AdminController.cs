using EMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace EmployeemanagementSystemCore.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<EmployeeDetail> employees = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7098/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var responseTask = client.GetAsync("api/Admin/GetAllEmployee");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmployeeDetail>>(readTask.Result);
                    readTask.Wait();
                    employees = deserialized;
                }
                else
                {
                    employees = Enumerable.Empty<EmployeeDetail>();
                    ModelState.AddModelError(string.Empty, "Employees not found.");
                }
            }
            return View(employees);
        }

        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddEmployee(EmployeeDetail employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7098/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var postTask = client.PostAsJsonAsync<EmployeeDetail>("api/Admin/AddEmployee", employee);
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Failed Try again.");
            return View(employee);
        }

        /*[HttpGet]
        public IActionResult GetEmployeeById(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7098/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var postTask = client.GetAsync($"api/Admin/GetEmployeeById/{id}");
                postTask.Wait();
                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    var deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<EmployeeDetail>(readTask.Result);
                    readTask.Wait();

                    return Json(deserialized);
                }
            }

            return RedirectToAction("Index");
        }*/

        [HttpGet]
        public IActionResult EditEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult EditEmployee(EmployeeDetail employee)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7098/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var putTask = client.PutAsJsonAsync($"api/Admin/EditEmployee/{employee.Id}", employee);

                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }

            ModelState.AddModelError(string.Empty, "Failed Try again.");
            return View(employee);
        }

        [HttpGet]
        public IActionResult DeleteEmployee()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteEmployee(Guid id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:7098/");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));
                    var deleteTask = client.DeleteAsync($"api/Admin/DeleteEmployee/{id}");

                    deleteTask.Wait();
                    var result = deleteTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                }
                return Json("\"Failed Try again.\"");
            }
            catch (Exception ex) { throw ex; }
        }
    }
}
