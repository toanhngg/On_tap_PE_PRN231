using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_PE3.DTO;
using Q1_PE3.Models;

namespace Q1_PE3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        public readonly PE_PRN_Fall22B1Context _context;

        public DirectorController(PE_PRN_Fall22B1Context context)
        {
            _context = context;
        }
        [HttpGet("GetDirectors/{nationality}/{gender}")]
        public IActionResult Get(string nationality, string gender)
        {
            try
            {
                bool isMale = true;
                if (gender.ToUpper() != "MALE") isMale = false;
                var listDirector = _context.Directors
                    .Where(x => x.Nationality == nationality && x.Male == true).
                    Select(x => new
                    {
                        id = x.Id,
                        fullName = x.FullName,
                        gender = x.Male ? "Male" : "Female",
                        dob = x.Dob,
                        dobString = x.Dob.ToString("dd/MM/yyyy"),
                        nationality = x.Nationality,
                        description = x.Description
                    }
                    )
                    .ToList();
                return Ok(listDirector);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetDirectors/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var listDirector = _context.Directors.Include(x => x.Movies)
                    .Where(x => x.Id == id).
                    Select(x => new
                    {
                        id = x.Id,
                        fullName = x.FullName,
                        gender = x.Male ? "Male" : "Female",
                        dob = x.Dob,
                        dobString = x.Dob.ToString("dd/MM/yyyy"),
                        nationality = x.Nationality,
                        description = x.Description,
                        movies = x.Movies.Select(y => new
                        {
                            y.Id,
                            y.Title,
                            y.ReleaseDate,
                            y.ReleaseDate.Value.Year,
                            y.Description,
                            y.Language,
                            y.ProducerId,
                            y.DirectorId,
                            y.Producer.Name,
                            x.FullName,
                            genres = new string[] { },
                            stars = new string[] { }
                        })
                    }
                    )
                    .ToList();
                return Ok(listDirector);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Create")]
        public IActionResult CreateDirector([FromBody]DirectorDTO director)
        {
            try
            {
                var directorId = _context.Directors.ToList().Where(
                    x => x.FullName.Equals(director.FullName)
                    && x.Male == director.Male
                    && x.Dob.Date == director.Dob.Date
                    && x.Nationality.Equals(director.Nationality)
                    && x.Description.Equals(director.Description)
                    ).FirstOrDefault();
                if (directorId != null) return Conflict("There is an error while adding."); 
                Director dir = new Director()
                {
                    FullName = director.FullName,
                    Male = director.Male,
                    Dob = director.Dob,
                    Nationality = director.Nationality,
                    Description = director.Description
                };
                _context.Directors.Add(dir);
                _context.SaveChanges();
                return Ok("1");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
