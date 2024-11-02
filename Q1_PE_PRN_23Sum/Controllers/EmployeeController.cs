using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_PE_PRN_23Sum.Models;

namespace Q1_PE_PRN_23Sum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public readonly PE_PRN_23SumContext _context;
        public EmployeeController(PE_PRN_23SumContext context)
        {
            _context = context;
        }
        [HttpGet("List/{minyear}/{maxyear}")]
        public IActionResult List(int minyear, int maxyear)
        {
            var listEmployee = _context.Employees.Include(x => x.ReportsToNavigation)
                           .Select(e => new
                           {
                               employeeId = e.EmployeeId,
                               lastName = e.LastName,
                               firstName = e.FirstName,
                               fullName = e.FirstName + " " + e.LastName,
                               title = e.Title,
                               titleOfCourtesy = e.TitleOfCourtesy,
                               birthDate = e.BirthDate,
                               birthDateStr = e.BirthDate.Value.ToString("dd/MM/yyyy"),
                               ReportsTo = e.ReportsToNavigation.FirstName == null || e.ReportsToNavigation.LastName == null ? "No one" : e.ReportsToNavigation.FirstName + " " + e.ReportsToNavigation.LastName,

                           }).Where(x => x.birthDate.Value.Year >= minyear && x.birthDate.Value.Year <= maxyear)
                           .ToList();

            return Ok(listEmployee);
        }
        [HttpGet("Index")]
        public IActionResult GetEmployee()
        {
            var listEmployee = _context.Employees.Include(x => x.ReportsToNavigation)
                           .Select(e => new
                           {
                               employeeId = e.EmployeeId,
                               lastName = e.LastName,
                               firstName = e.FirstName,
                               fullName = e.FirstName + " " + e.LastName,
                               title = e.Title,
                               titleOfCourtesy = e.TitleOfCourtesy,
                               birthDate = e.BirthDate,
                               birthDateStr = e.BirthDate.Value.ToString("dd/MM/yyyy"),
                               ReportsTo = e.ReportsToNavigation.FirstName == null || e.ReportsToNavigation.LastName == null ? "No one" : e.ReportsToNavigation.FirstName + " " + e.ReportsToNavigation.LastName,

                           })
                           .ToList();

            return Ok(listEmployee);
        }

        [HttpGet("Delete")]
        public IActionResult DeleteById(int id)
        {
            try
            {
                var employee = _context.Employees.Include(x => x.ReportsToNavigation)
                    .Include(e => e.Orders)
                    .ThenInclude(o => o.OrderDetails)
                    .Where(x => x.EmployeeId == id).FirstOrDefault();
                if (employee != null)
                {
                    _context.Employees.Remove(employee);
                    _context.SaveChanges();
                    return Ok(employee);
                }return NotFound("The request employee could be found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
