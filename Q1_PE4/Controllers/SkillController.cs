using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_PE4.Models;

namespace Q1_PE4.Controllers
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
        public IActionResult Get()
        {
            try {
                var skilllist = _context.Skills.ToList()
                    .Select(x => new
                    {
                        skillId = x.SkillId,
                        skillName = x.SkillName,
                        description = x.Description,
                        numberOfEmployee = _context.EmployeeSkills
                        .Where(y => y.SkillId == x.SkillId).Count()
                    }).ToList();
                return Ok(skilllist);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSkills/{skillId}")]
        public IActionResult GetSkills(int skillId)
        {
            try
            {
                var skilllist = _context.Skills.ToList().
                    Where(x => x.SkillId == skillId)
                    .Select(x => new
                    {
                        skillId = x.SkillId,
                        skillName = x.SkillName,
                        description = x.Description,
                        employees = _context.EmployeeSkills.Include(e => e.Skill)
                        .Where(x => x.SkillId.Equals(skillId))
                        .Select(
                            y => new
                            {
                                employeeId = y.EmployeeId,
                                employeeName = y.Employee.Name,
                                department = y.Employee.Department,
                                proficiencyLevel = y.ProficiencyLevel, 
                                acquiredDate = y.AcquiredDate

                            }).ToList()
                    }).ToList();
                return Ok(skilllist);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
