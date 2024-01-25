using EMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace EmployeemanagementSystemCore.Controllers
{
    public class ManagerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<EmployeeDetail> employees = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7098/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var responseTask = client.GetAsync("api/Manager/GetAllEmployee");
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
        public IActionResult AddFeedback()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFeedback(EmployeeDetail employeeDetail)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:7098/");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", HttpContext.Session.GetString("Token"));

                var json = JsonConvert.SerializeObject(employeeDetail);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var putTask = client.PutAsync($"api/Manager/AddFeedback/{employeeDetail.Id}", content);


                putTask.Wait();
                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Failed Try again.");
            return View(employeeDetail);
        }
    }
}
