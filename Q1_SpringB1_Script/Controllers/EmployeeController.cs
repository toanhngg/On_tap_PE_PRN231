using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_SpringB1_Script.Models;

namespace Q1_SpringB1_Script.Controllers
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
        [HttpDelete("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {

                int nProject = 0;
                int nSkill = 0;
                int nDepartment = 0;
                var employee = _context.Employees.Where(x => x.EmployeeId == id).FirstOrDefault();
                if (employee == null) return NotFound("No employee has id" + id);

                var list = _context.EmployeeProjects.Where(x => x.Employee.EmployeeId == id).ToList();
                var list2 = _context.EmployeeSkills.Where(x => x.Employee.EmployeeId == id).ToList();
                _context.EmployeeProjects.RemoveRange(list);
                _context.EmployeeSkills.RemoveRange(list2);

                var department = _context.Departments.Include(x => x.Employees).Where(x => x.DepartmentId == employee.DepartmentId).ToList();
                _context.Departments.RemoveRange(department);
                _context.Employees.Remove(employee);
                _context.SaveChanges();
                nProject = list.Count();
                nSkill = list2.Count();
                nDepartment = department.Count();

                var response = new
                {
                    numberOfProject = nProject,
                    numberOfSkill = nSkill,
                    numberOfDepartment = nDepartment,
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
