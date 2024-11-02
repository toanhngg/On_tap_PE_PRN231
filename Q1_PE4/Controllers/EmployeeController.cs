using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_PE4.Models;

namespace Q1_PE4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        public readonly SpringB1_ScriptContext _context;

        public EmployeeController(SpringB1_ScriptContext context)
        {
            _context = context;
        }
        [HttpDelete("Delete")]
        public IActionResult Delete([FromQuery] int EmployeeId)
        {
            try
            {
                int numberOfProject = 0;
                int numberOfSkills = 0;
                int numberOfDepartment = 0;

                var employee = _context.Employees
                    .ToList().Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
                if (employee == null) return NotFound("No employee whith id" + EmployeeId);

                var numberProject = _context.EmployeeProjects.ToList()
                   .Where(x => x.EmployeeId == EmployeeId);
                numberOfProject = numberProject.Count();

                var numberSkill = _context.EmployeeSkills.ToList()
            .Where(x => x.EmployeeId == EmployeeId);
                numberOfSkills = numberSkill.Count();

                var department = _context.Departments
            .Where(x => x.DepartmentId == employee.DepartmentId).ToList();

                //var department = _context.Departments.Include(x => x.Employees)
                //    .Where(x => x.DepartmentId == employee.DepartmentId).FirstOrDefault();

                numberOfDepartment = department.Count();
                _context.Departments.RemoveRange(department);
                _context.EmployeeSkills.RemoveRange(numberSkill);
                _context.EmployeeProjects.RemoveRange(numberProject);
                _context.Employees.Remove(employee);
                var response = new
                {
                    numberOfProject,
                    numberOfSkills,
                    numberOfDepartment
                };
                // _context.SaveChanges();
                return Ok(response);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("Delete2")]
        public IActionResult Delete2([FromQuery] int EmployeeId)
        {
            try
            {
                int numberOfProject = 0;
                int numberOfSkills = 0;
                int numberOfDepartment = 0;

                var employee = _context.Employees
                    .ToList().Where(x => x.EmployeeId == EmployeeId).FirstOrDefault();
                if (employee == null) return NotFound("No employee whith id" + EmployeeId);

                var numberProject = _context.EmployeeProjects.ToList()
                   .Where(x => x.EmployeeId == EmployeeId);
                numberOfProject = numberProject.Count();

                var numberSkill = _context.EmployeeSkills.ToList()
            .Where(x => x.EmployeeId == EmployeeId);
                numberOfSkills = numberSkill.Count();

                var department = _context.Departments
            .Where(x => x.DepartmentId == employee.DepartmentId).ToList();
                numberOfDepartment = department.Count();

                //var department = _context.Departments.Include(x => x.Employees)
                //    .Where(x => x.DepartmentId == employee.DepartmentId).FirstOrDefault();

                employee.Departments.Clear();
                employee.EmployeeSkills.Clear();
                employee.EmployeeProjects.Clear();
                _context.Employees.Remove(employee);
                var response = new
                {
                    numberOfProject,
                    numberOfSkills,
                    numberOfDepartment
                };
                 _context.SaveChanges();
                return Ok(response);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
