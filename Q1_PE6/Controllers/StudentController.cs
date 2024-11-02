using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_PE6.Models;
using System.Linq;

namespace Q1_PE6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        public readonly DUONGPV14_PRNContext _context;

        public StudentController(DUONGPV14_PRNContext context)
        {
            _context = context;
        }
        [HttpGet("List")]
        public IActionResult List()
        {
            try
            {
                var listStudent = _context.Students.Include(x => x.Lecture)
                    .Select(x => new
                    {
                        id = x.Id,
                        fullName = x.FullName,
                        gender = x.Male ? "Male" : "Female",
                        dob = x.Dob.ToString("dd/MM/yyyy"),
                        lectureName = x.Lecture.FullName,
                        age = DateTime.Now.Year - x.Dob.Year
                    }).ToList();
                return Ok(listStudent);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("Detail")]
        public IActionResult List([FromQuery] int id)
        {
            try
            {
                //var listStudent = _context.Students.Include(x => x.Lecture)
                //                  .Where(x => x.Id == id)
                //                  .Select(x => new
                //                  {
                //                      id = x.Id,
                //                      fullName = x.FullName,
                //                      gender = x.Male ? "Male" : "Female",
                //                      dob = x.Dob.ToString("dd/MM/yyyy"),
                //                      lectureName = x.Lecture.FullName,
                //                      age = DateTime.Now.Year - x.Dob.Year,
                //                      classes = _context.Classes.Where(c => c.Students.Any(s => s.Id == id)).Select(c => c.ClassName.Trim()).ToList(),
                //                      subjects = _context.StudentSubjects.Where(ss => ss.StudentId == id).Select(ss=> ss.Subject.Subject1.Trim()).ToList(),
                //                      cpa = _context.StudentSubjects.Where(ss => ss.StudentId == id)
                //                      .Select(ss => ss.Grade).Average()
                //                  }).ToList();

                //var listStudent = _context.Students
                //                         .Include(x => x.Lecture)
                //                         .Where(x => x.Id == id)
                //                         .Select(x => new
                //                         {
                //                             id = x.Id,
                //                             fullName = x.FullName,
                //                             gender = x.Male ? "Male" : "Female",
                //                             dob = x.Dob.ToString("dd/MM/yyyy"),
                //                             lectureName = x.Lecture.FullName,
                //                             age = DateTime.Now.Year - x.Dob.Year,
                //                             classes = _context.Classes
                //                                 .Where(c => c.Students.Any(s => s.Id == id))
                //                                 .Select(c => c.ClassName.Trim())
                //                                 .ToList(),
                //                             subjects = _context.StudentSubjects
                //                                 .Where(ss => ss.StudentId == id)
                //                                 .Select(ss => ss.Subject.Subject1.Trim())
                //                                 .ToList(),
                //                             cpa = _context.StudentSubjects
                //                                 .Where(ss => ss.StudentId == id)
                //                                 .AsEnumerable()  
                //                                 .Sum(ss => ss.Grade)
                //                         })
                //                         .ToList();
                var listStudent = _context.Students
                                        .Include(x => x.Lecture)
                                        .Where(x => x.Id == id)
                                        .Select(x => new
                                        {
                                            id = x.Id,
                                            fullName = x.FullName,
                                            gender = x.Male ? "Male" : "Female",
                                            dob = x.Dob.ToString("dd/MM/yyyy"),
                                            lectureName = x.Lecture.FullName,
                                            age = DateTime.Now.Year - x.Dob.Year,
                                            classes = _context.Classes.AsEnumerable()
                                                .Where(c => c.Students.Any(s => s.Id == id))
                                                .Select(c => c.ClassName.Trim())
                                                .Count(),
                                            subjects = _context.StudentSubjects.AsEnumerable()
                                                .Where(ss => ss.StudentId == id)
                                                .Select(ss => ss.Subject.Subject1.Trim())
                                                .Count(),
                                            cpa = _context.StudentSubjects
                                                .Where(ss => ss.StudentId == id)
                                                .AsEnumerable()
                                                .Average(ss => ss.Grade)
                                        })
                                        .ToList();
                return Ok(listStudent);


            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Delete")]
        public IActionResult Delete([FromQuery]int id)
        {
            try
            {
                var student = _context.Students
                                      .Include(x => x.Classes)
                                      .Include(x => x.StudentSubjects)
                                      .Include(x => x.Lecture).Where(x => x.Id == id).FirstOrDefault();
                int studentSubject = _context.StudentSubjects.AsEnumerable()
                                                .Where(ss => ss.StudentId == id)
                                                .Select(ss => ss.Subject)
                                                .Count();
                int classRelate = _context.Classes.AsEnumerable()
                                                .Where(c => c.Students.Any(s => s.Id == id))
                                                .Select(c => c.ClassName.Trim())
                                                .Count();
                student.Classes.Clear();
                student.StudentSubjects.Clear();
                _context.Students.Remove(student);
                var response = new
                {
                    umberOfRelatedClasses = classRelate,
                    numberOfRelatedSubjects = studentSubject
                };
 

                //_context.SaveChanges();
                return Ok(response);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
