using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCRUD.Models;
using MovieCRUD.ViewModels;
using System.IO;

namespace MovieCRUD.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDBContext _dbcontext;
        private readonly IWebHostEnvironment _webHost;
        private new List<string> _allowedExtenstions = new List<string> { ".jpg", ".png" ,".jpeg"};
        private long _maxAllowedPosterSize = 1048576;

        public MovieController(ApplicationDBContext  dBContext,IWebHostEnvironment webHost)
        {
            _dbcontext = dBContext;
            _webHost = webHost;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _dbcontext.Movies.Include(e => e.Genre).ToListAsync());
        }

        #region CreateGet
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var ViewModel = new MovieFormViewModel()
            {
                Genres = await _dbcontext.Genres.OrderBy(x => x.Name).ToListAsync()
            };
            return View(ViewModel);
        }
        #endregion

        #region CreatePost
        [HttpPost]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            model.Genres = await _dbcontext.Genres.ToListAsync();

            if (!ModelState.IsValid)
                return View(model);


            if (model.File is  null)
            {
                ModelState.AddModelError("File", "Please select movie poster!");
                return View(model);
            }

            string ext = Path.GetExtension(model.File.FileName);

            if (!_allowedExtenstions.Contains(Path.GetExtension(ext).ToLower()))
            {
                model.Genres = await _dbcontext.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("File", "Only .PNG, .JPG images are allowed!");
                return View(model);
            }

            if (model.File.Length > _maxAllowedPosterSize)
            {
                model.Genres = await _dbcontext.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("File", "Poster cannot be more than 1 MB!");
                return View(model);
            }

            var imagName = $"{Guid.NewGuid()}{ext}";
            var path = Path.Combine(_webHost.WebRootPath, "uploads");
            var filepath = Path.Combine(path, imagName);

            using var stream = System.IO.File.Create(filepath);
            model.File.CopyTo(stream);

            Movie movie = new Movie()
            {
                Title = model.Title,
                Rate = model.Rate,
                StoreLine = model.StoreLine,
                year = model.year,
                GenreId = model.Genreid,
                ImgUrl = imagName,
            };

            await _dbcontext.AddAsync(movie);
            await _dbcontext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region EditGet
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _dbcontext.Movies.FindAsync(id);

            if (movie == null)
                return NotFound();

            MovieFormViewModel movieVM = new MovieFormViewModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                StoreLine = movie.StoreLine,
                year = movie.year,
                Rate = movie.Rate,
                Genreid = movie.GenreId,
                ImgUrl = movie.ImgUrl,
                Genres = _dbcontext.Genres.ToList()
            };
            return View(movieVM);
        }
        #endregion

        #region EditPost
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            model.Genres = await _dbcontext.Genres.ToListAsync();
            if (!ModelState.IsValid)
                return View(model);

            var files = model.File;

            string ext = Path.GetExtension(model.File.FileName);

            if (!_allowedExtenstions.Contains(Path.GetExtension(ext).ToLower()))
            {
                ModelState.AddModelError("File", "Only .PNG, .JPG images are allowed!");
                return View(model);
            }

            if (model.File.Length > _maxAllowedPosterSize)
            {
                ModelState.AddModelError("File", "Poster cannot be more than 1 MB!");
                return View(model);
            }

            var imagName = $"{Guid.NewGuid()}{ext}";
            var path = Path.Combine(_webHost.WebRootPath, "uploads");
            var filepath = Path.Combine(path, imagName);

            using var stream = System.IO.File.Create(filepath);
            model.File.CopyTo(stream);

            Movie movie = new Movie()
            {
                Id = model.Id,
                Title = model.Title,
                StoreLine = model.StoreLine,
                GenreId = model.Genreid,
                year = model.year,
                Rate = model.Rate,
                ImgUrl = imagName
            };
             _dbcontext.Update(movie);
            await _dbcontext.SaveChangesAsync();

            return View(nameof(Index),await _dbcontext.Movies.OrderBy(e => e.Title).ToListAsync());

        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int id)
        {
            var movie = await _dbcontext.Movies.FindAsync(id);
            if (movie is null)
                return NotFound();

            MovieFormViewModel model = new MovieFormViewModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                StoreLine = movie.StoreLine,
                Genreid = movie.GenreId,
                year = movie.year,
                Rate = movie.Rate,
                ImgUrl = movie.ImgUrl,
            };

            return View(model);
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return NotFound();

            var movie = await _dbcontext.Movies.FindAsync(id);
            if (movie is null)
                return NotFound();

            _dbcontext.Remove(movie);
            await _dbcontext.SaveChangesAsync();

            return Ok();
        }
        #endregion
    }
}
