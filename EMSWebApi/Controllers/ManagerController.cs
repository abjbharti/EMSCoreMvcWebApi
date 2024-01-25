using EMSModels;
using EMSWebApi.Context;
using EMSWebApi.GenericRepo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace EMSWebApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "Manager")]
    [Route("api/[controller]")]
    public class ManagerController : Controller
    {
        public IGenericRepository<EmployeeDetail> _genericRepository = null!;
        public ApplicationDbContext _dbContext;

        public ManagerController(IGenericRepository<EmployeeDetail> genericRepository, ApplicationDbContext dbContext)
        {
            _genericRepository = genericRepository;
            _dbContext = dbContext;
        }

        private static Expression<Func<EmployeeDetail, object>> GetPropertyExpression(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(EmployeeDetail), "x");
            var property = Expression.Property(parameter, propertyName);
            var convert = Expression.Convert(property, typeof(object));
            var lambda = Expression.Lambda<Func<EmployeeDetail, object>>(convert, parameter);
            return lambda;
        }

        [HttpGet("GetAllEmployee")]
        public async Task<IActionResult> GetAllEmployee(int page = 1, int pageSize = 5, string? search = "", string? sort = "", string? column = "")
        {
            var query = _dbContext.EmployeeDetails.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Name.Contains(search));
            }

            var totalCount = await query.CountAsync();
            var totalPage = (int)Math.Ceiling((decimal)totalCount / pageSize);

            if (string.IsNullOrEmpty(column))
            {
                column = "Name";
            }

            switch (sort)
            {
                case "asc":
                    query = query.OrderByDescending(GetPropertyExpression(column));
                    sort = "asc";
                    break;
                default:
                    query = query.OrderBy(GetPropertyExpression(column));
                    sort = "desc";
                    break;
            }

            var employeePerPage = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var response = new
            {
                TotalCount = totalCount,
                TotalPage = totalPage,
                EmployeePerPage = employeePerPage
            };

            return Ok(employeePerPage);
        }

        [HttpPut("AddFeedback/{id:guid}", Name = "AddFeedback")]
        public async Task<IActionResult> AddFeedback(Guid id, AddFeedbackByManager addFeedbackByManager)
        {
            if (addFeedbackByManager is null)
            {
                throw new ArgumentNullException(nameof(addFeedbackByManager));
            }

            var employee = await _dbContext.EmployeeDetails.FindAsync(id);

            if (employee != null)
            {
                employee.Feedback = addFeedbackByManager.Feedback;
                await _dbContext.SaveChangesAsync();
                return Ok(employee);
            }

            return NotFound();
        }
    }
}
