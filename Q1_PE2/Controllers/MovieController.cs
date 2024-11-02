using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Q1_PE2.Models;

namespace Q1_PE2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        public readonly PE_PRN231_Trial_02Context _context;

        public MovieController(PE_PRN231_Trial_02Context context)
        {
            _context = context;
        }
        [HttpPost("RemoveStarFromMovie/{movieId}/{starId}")]
        public IActionResult RemoveStarFromMovie(int movieId, int starId )
        {
            try
            {
                //var movie = _PRN221_Trial3Context.Movies.Include(s => s.Stars).Include(g => g.Genres).FirstOrDefault(s => s.Id == id);

                //// Cach 1
                ////_PRN221_Trial3Context.Database.ExecuteSqlRaw($"delete from Movie_Star where MovieId = {id}" +
                ////$"delete from Movie_Genre where MovieId = {id}" +
                ////$"delete from Movies where Id = {id}");

                //// Cach 2
                ////movie.Stars = null;
                ////movie.Genres = null;
                ////_PRN221_Trial3Context.Update(movie);
                ////_PRN221_Trial3Context.SaveChanges();
                ////_PRN221_Trial3Context.Remove(movie);
                ////_PRN221_Trial3Context.SaveChanges();

                //// Cach 3
                ////movie.Stars.Clear();
                ////movie.Genres.Clear();

                //_PRN221_Trial3Context.Remove(movie);
                //_PRN221_Trial3Context.SaveChanges();
                //return Ok();

                var movies = _context.Movies.Where(x => x.Id == movieId).FirstOrDefault();
                if (movies == null) return NotFound("The requested movie could not be found.");
                var movie = _context.Stars.Include(x => x.Movies).Where(x => x.Id == starId).FirstOrDefault(
                    x => x.Movies.Any(m => m.Id == movieId));
                if (movie == null) return NotFound();

                movie.Movies.Remove(movies);
                _context.Stars.Remove(movie);
                _context.SaveChanges();
                
                return Ok();


            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
