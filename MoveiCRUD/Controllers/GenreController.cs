using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCRUD.Models;

namespace MovieCRUD.Controllers
{
    public class GenreController : Controller
    {
        private readonly ApplicationDBContext _dBContext;

        public GenreController(ApplicationDBContext dBContext)
        {
            _dBContext = dBContext;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var genres = await _dBContext.Genres.ToListAsync();
            return View(genres);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Genre genre)
        {
            if (!ModelState.IsValid)
                return View(genre);

            await _dBContext.Genres.AddAsync(genre);
            await _dBContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return NotFound();

            var genre = await _dBContext.Genres.FindAsync(id);
            
            if(genre is null)
                return NotFound();

            return View(genre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Genre genre)
        {
            if(!ModelState.IsValid)
                return View(genre);

            _dBContext.Genres.Update(genre);
            await _dBContext.SaveChangesAsync();
            
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id is null)
                return NotFound();

            var genre = await _dBContext.Genres.FindAsync(id);

            if (genre is null)
                return NotFound();

            genre.Movies = await _dBContext.Movies.Where(e => e.GenreId == id).ToListAsync();

            if(genre.Movies.Any())
            {
                return View(genre);
            }    

            _dBContext.Remove(genre);
            await _dBContext.SaveChangesAsync();
            return Ok();  

        }

    }
}
