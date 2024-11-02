using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_PE5.Models;

namespace Q1_PE5.Controllers
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
        [HttpGet("Index")]
        public IActionResult Get()
        {
            try
            {
                var employee = _context.Employees.Include(x => x.ReportsToNavigation)
                    .Select(x => new
                    {
                        employeeId = x.EmployeeId,
                        lastName = x.LastName,
                        firstName = x.FirstName,
                        fullName = x.FirstName + " " + x.LastName,
                        title = x.Title,
                        titleOfCourtesy = x.TitleOfCourtesy,
                        birthDate = x.BirthDate,
                        birthDateStr = x.BirthDate.Value.ToString("dd/MM/yyyy"),
                        ReportsTo = x.ReportsToNavigation.FirstName == null || 
                        x.ReportsToNavigation.LastName == null ? "No one" : 
                        x.ReportsToNavigation.FirstName + " " + x.ReportsToNavigation.LastName,
                        //reportTo = _context.Employees.Where( y=> y.EmployeeId == x.ReportsTo).Select(y=> y.FirstName)
                    }).ToList();
                return Ok(employee);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Index/{minyear}/{maxyear}")]
        public IActionResult List(int minyear, int maxyear)
        {
            try
            {
                var employee = _context.Employees.Include(x => x.ReportsToNavigation)
                    .Where(x=> x.BirthDate.Value.Year > minyear && x.BirthDate.Value.Year < maxyear)
                    .Select(x => new
                    {
                        employeeId = x.EmployeeId,
                        lastName = x.LastName,
                        firstName = x.FirstName,
                        fullName = x.FirstName + " " + x.LastName,
                        title = x.Title,
                        titleOfCourtesy = x.TitleOfCourtesy,
                        birthDate = x.BirthDate,
                        birthDateStr = x.BirthDate.Value.ToString("dd/MM/yyyy"),
                        ReportsTo = x.ReportsToNavigation.FirstName == null ||
                        x.ReportsToNavigation.LastName == null ? "No one" :
                        x.ReportsToNavigation.FirstName + " " + x.ReportsToNavigation.LastName,
                        //reportTo = _context.Employees.Where( y=> y.EmployeeId == x.ReportsTo).Select(y=> y.FirstName)
                    }).ToList();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")] 
        public IActionResult Delete(int id)
        {
            try
            {
                var employee = _context.Employees.
                    Include(x => x.ReportsToNavigation)
                    .Include(x=> x.Orders).ThenInclude(x => x.OrderDetails)
                    .ToList().Where(x=>  x.EmployeeId == id).FirstOrDefault();
                if (employee == null) return NotFound("The requested employee could be found");
                _context.Employees.Remove(employee);
                _context.SaveChanges();

                return Ok();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
