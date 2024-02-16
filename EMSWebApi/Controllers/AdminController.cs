using EMSModels;
using EMSWebApi.Context;
using EMSWebApi.GenericRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EMSWebApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        public IGenericRepository<EmployeeDetail> _genericRepository;
        
        public AdminController(IGenericRepository<EmployeeDetail> genericRepository, ApplicationDbContext dbContext)
        {
            _genericRepository = genericRepository;
        }


        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployee()
        {
            var result = await _genericRepository.GetAll();

            if (result != null)
            {
                return Ok(result);
            }
            return NotFound();
        }


        /*[HttpGet("{id:guid}", Name = "GetEmployee")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var result = await _genericRepository.GetById(id);

            if (result != null)
            {
                return new JsonResult((EmployeeDetail)result);
            }
            return NotFound();
        }*/


        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee(AddEmployeeByAdmin addEmployeeByAdmin)
        {
            var employee = new EmployeeDetail
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeByAdmin.Name,
                Department = addEmployeeByAdmin.Department
            };

            await _genericRepository.Insert(employee);
            return Ok();
        }


        [HttpPut("EditEmployee/{id:guid}", Name = "EditEmployee")]
        public async Task<IActionResult> EditEmployee(EmployeeDetail employeeDetail)
        {
            if (employeeDetail is null)
            {
                throw new ArgumentNullException(nameof(employeeDetail));
            }

            var employee = await _genericRepository.GetById(employeeDetail.Id);

            if (employee != null)
            {
                employee.Name = employeeDetail.Name;
                employee.Department = employeeDetail.Department;
                await _genericRepository.Update(employee);
                return Ok(employee);
            }

            return NotFound();
        }


        [HttpDelete("DeleteEmployee/{id:guid}", Name = "DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
           var employee = await _genericRepository.GetById(id);

            if (employee != null)
            {
                await _genericRepository.Delete(id);
                return Ok(employee);
            }

            return NotFound();
        }
    }
}
