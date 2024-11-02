using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Q1_SpringB1_Script.Models;

namespace Q1_SpringB1_Script.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        public readonly SpringB1_ScriptContext _context;
        public SkillController(SpringB1_ScriptContext context)
        {
            _context = context;
        }
        [HttpGet("GetSkills")]
        public IActionResult GetSkills()
        {
            try
            {
                var listSkill = (from s in _context.Skills
                                 join e in _context.EmployeeSkills on s.SkillId equals e.SkillId
                                 select new
                                 {
                                     skillId = s.SkillId,
                                     skillName = s.SkillName,
                                     description = s.Description,
                                     numberOfEmployee = s.EmployeeSkills.Select(x => x.SkillId).Count()
                                 }
                                 ).ToList();
                return Ok(listSkill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetSkills/{SkillId}")]
        public IActionResult GetSkillsByID(int SkillId)
        {
            try
            {
                var listSkill = (from s in _context.Skills
                                 join e in _context.EmployeeSkills on s.SkillId equals e.SkillId
                                 select new
                                 {
                                     skillId = s.SkillId,
                                     skillName = s.SkillName,
                                     description = s.Description,
                                     employee =
                                     (from e1 in _context.Employees
                                      join es in _context.EmployeeSkills on e1.EmployeeId equals es.EmployeeId
                                      join d in _context.Departments on e1.DepartmentId equals d.DepartmentId
                                      where es.SkillId == SkillId
                                      select new
                                      {
                                          employeeId = es.EmployeeId,
                                          employeeName = e1.Name,
                                           department = e1.Department.DepartmentName,
                                           proficiencyLevel = es.ProficiencyLevel,
                                           acquiredDate = es.AcquiredDate,
                                      }).ToList()
                                 }
                                 ).ToList().Where(s=> s.skillId == SkillId).FirstOrDefault();
                if (listSkill == null) return StatusCode(404); // Trả về mã trạng thái 404 mà không có nội dung
                return Ok(listSkill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
