using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Q1_PE_PRN231_Trial_02.Models;

namespace Q1_PE_PRN231_Trial_02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorController : ControllerBase
    {
        PE_PRN231_Trial_02Context _context;
        public DirectorController(PE_PRN231_Trial_02Context context)
        {
            _context = context;
        }
        [HttpGet("GetDirectors/{nationality}/{gender}")]
        public IActionResult GetDirectors(string nationality, string gender)
        {
            try
            {
                bool isMale = true; // Or false, based on your logic
                //var listDirector = _context.Directors
                //    .Where(x => x.Nationality == nationality && x.Male == isMale)
                //    .Select(x => new
                //    {
                //        x.Name, // Assuming there's a Name property or any other properties you need
                //        Gender = x.Male ? "Male" : "Female"
                //    })
                //    .ToList();
                DateTime dateTime;
                var listDirector = from d in _context.Directors
                                   where d.Nationality == nationality && d.Male == isMale
                                   select new
                                   {
                                       id = d.Id,
                                       fullName = d.FullName, // Assuming there's a Name property or any other properties you need
                                       gender = d.Male ? "Male" : "Female",
                                       dob = d.Dob,
                                       DobString = d.Dob.ToString("dd/MM/yyyy"),
                                       nationality = d.Nationality,
                                       description = d.Description,

                                   };
                return Ok(listDirector);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDirectors/{id}")]
        public IActionResult GetDirectors( int id)
        {
            try
            {
                if (id == null) return BadRequest(string.Empty);
                //var listDirector = from d in _context.Directors
                //                   join m in _context.Movies on d.Id equals m.DirectorId
                //                   where d.Id == id
                //                   select new
                //                   {
                //                       id = d.Id,
                //                       fullName = d.FullName, 
                //                       gender = d.Male ? "Male" : "Female",
                //                       dob = d.Dob,
                //                       DobString = d.Dob.ToString("dd/MM/yyyy"),
                //                       nationality = d.Nationality,
                //                       description = d.Description,
                //                       movies = (from m in _context.Movies
                //                                 join de in _context.Directors on  m.DirectorId equals de.Id 
                //                                 where m.DirectorId == d.Id
                //                                 select new
                //                                 {
                //                                     id = m.Id,
                //                                     title = m.Title,
                //                                     releaseDate = m.ReleaseDate,
                //                                     releaseYear = m.ReleaseDate.Value.ToString("dd/MM/yyyy"),
                //                                     description = m.Description,
                //                                     language = m.Language,
                //                                     producerId = m.Producer.Id,
                //                                     directorId = m.Director.Id,
                //                                     producerName = m.Producer.Name,
                //                                     directorName = m.Director.FullName,
                //                                     genres = new string[] { }, // Empty array
                //                                     stars = new string[] { }  // Empty array
                //                                     //generes = m.Genres.Select(g => new { }).ToList(),
                //                                     //stars = m.Stars.Select(s => new  {}).ToList()
                //                                 }
                //                       ).ToList()
                //                   };
                var result = from d in _context.Directors
                             where d.Id == id // Replace 5 with your actual director ID
                             select new
                             {
                                 id = d.Id,
                                 fullName = d.FullName,
                                 gender = d.Male ? "Male" : "Female",
                                 dob = d.Dob.Date,
                                 dobString = d.Dob.ToString("yyyy-MM-dd"),
                                 nationality = d.Nationality,
                                 description = d.Description
                                 ,
                                 Movies = (from m in _context.Movies
                                           join p in _context.Producers on m.ProducerId equals p.Id
                                           where m.DirectorId == d.Id
                                           select new
                                           {
                                               movieId = m.Id,
                                               title = m.Title,
                                               releaseDate = m.ReleaseDate,
                                               description = m.Description,
                                               language = m.Language,
                                               producerId = m.ProducerId,
                                               producerName = p.Name,
                                               directorsName = d.FullName,

                                               genres = new string[] { }, // Empty array
                                               stars = new string[] { }  // Empty array
                                           }).ToList()
                             };
                return Ok(result.FirstOrDefault());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
