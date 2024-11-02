using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_PE_PRN231_Trial_02.Models;

namespace Q1_PE_PRN231_Trial_02.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        PE_PRN231_Trial_02Context _context;
        public MovieController(PE_PRN231_Trial_02Context context)
        {
            _context = context;
        }
        [HttpDelete("RemoveStarFromMovie/{movieId}/{starId}")]
        public ActionResult RemoveStarFromMovie(int movieId,int starId) {
            try
            {
                var movies = _context.Movies.Where(x => x.Id == movieId).FirstOrDefault();
                if(movies == null ) return NotFound("The requested movie could not be found.");
                //var star = _context.Stars.Where(x => x.Id == starId).ToList();
                //if (starId == 0) return NotFound("The requested actor could not be found in the list of actors of the requested movie");
   //             var stars = _dbContext.Stars.Include(s => s.Movies).
   //FirstOrDefault(s => s.Id == starId && s.Movies.Any(m => m.Id == movieId));

                var moviesStar = _context.Stars.Include(m => m.Movies).
                    FirstOrDefault(x => x.Id == starId && x.Movies.Any(m => m.Id == movieId));
                if (moviesStar == null)
                {
                    return NotFound("The requested actor could not be found in the list of actors of the requested movie.");
                }
                moviesStar.Movies.Remove(movies);
                _context.SaveChanges();
                _context.Stars.Remove(moviesStar);
                _context.SaveChanges();
                return Ok();


            }
            catch (Exception ex) { 
            return Conflict(ex.Message);
            }
        }

        [HttpGet("GetStar")]
        public IActionResult Get(int movieId)
        {

            var stars = _context.Stars.Include(s => s.Movies)
                .Where(star => star.Movies.Any(movie => movie.Id == movieId))
                .Select(star => new
                {
                    star.Id
                })
                .ToList();

            return Ok(stars);
        }
    }
}
