using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Q1_PE_PRN_Fall22B1.DTO;
using Q1_PE_PRN_Fall22B1.Models;

namespace Q1_PE_PRN_Fall22B1.Controllers
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
        [HttpGet("GetDirectors/{national}/{gender}")]
        public async Task<IActionResult> GetDirectors(string national, string gender)
        {
            try
            {
                bool check = true;

                if (gender.ToLower() != "male")
                {
                    check = false;
                }
                var lisDirector = from d in _context.Directors
                                  where d.Nationality == national && d.Male == check
                                  select new
                                  {

                                      id = d.Id,
                                      fullName = d.FullName, // Assuming there's a Name property or any other properties you need
                                      gender = d.Male ? "Male" : "Female",
                                      dob = d.Dob,
                                      DobString = d.Dob.ToString("dd/MM/yyyy"),
                                      nationality = d.Nationality,
                                      description = d.Description
                                  };
                return Ok(lisDirector);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDirectors/{id}")]
        public async Task<IActionResult> GetDirectorsById(int id)
        {
            try
            {
                var lisDirector = (from d in _context.Directors
                                   join m in _context.Movies on d.Id equals m.DirectorId
                                   where d.Id == id
                                   select new
                                   {
                                       id = d.Id,
                                       fullName = d.FullName, // Assuming there's a Name property or any other properties you need
                                       gender = d.Male ? "Male" : "Female",
                                       dob = d.Dob,
                                       DobString = d.Dob.ToString("dd/MM/yyyy"),
                                       nationality = d.Nationality,
                                       description = d.Description,
                                       movies = (from m in _context.Movies
                                                 join p in _context.Producers on m.ProducerId equals p.Id
                                                 join d in _context.Directors on  m.DirectorId equals d.Id
                                                 where d.Id == id
                                                 select new
                                                 {
                                                     id = m.Id,
                                                     title = m.Title,
                                                     releaseDate = m.ReleaseDate,
                                                     releaseYear = m.ReleaseDate.Value.ToString("yyyy"),
                                                     description = m.Description,
                                                     language = m.Language,
                                                     producerId = m.ProducerId,
                                                     producerName = p.Name,
                                                     directorsName = d.FullName,
                                                     genres = new string[] { }, // Empty array
                                                     stars = new string[] { }  // Empty array

                                                 }
                                                 ).ToList()
                                   }).FirstOrDefault();
                return Ok(lisDirector);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateDirector([FromBody] DirectorDTO dr)
        {
            try
            {
                if (dr != null)
                {

                    var dir = _context.Directors
                   .ToList()  // Materialize the query
                   .Where(x => x.FullName.Equals(dr.FullName)
                               && x.Dob.Date == dr.Dob.Date
                               && x.Nationality.Equals(dr.Nationality)
                               && x.Description.Equals(dr.Description))
                   .FirstOrDefault();


                    if (dir == null)
                    {
                        Director director = new Director()
                        {
                            FullName = dr.FullName,
                            Male = dr.Male,
                            Dob = dr.Dob,
                            Nationality = dr.Nationality,
                            Description = dr.Description,
                        };
                        _context.Directors.Add(director);
                        _context.SaveChanges();
                        return Ok("1");
                    }
                    else
                    {
                        return Conflict("There is a conflict while adding");

                    }
                }
                return BadRequest("Chua nhap du lieu");

            }
            catch (Exception ex)
            {
                return Conflict(ex.Message);
            }
        }
    }
}
