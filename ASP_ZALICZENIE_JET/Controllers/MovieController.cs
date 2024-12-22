using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP_ZALICZENIE_JET.Models.Movies;

namespace ASP_ZALICZENIE_JET.Controllers
{
    
    public class MovieController : Controller
    {
        private readonly MoviesDbContext _context;

        public MovieController(MoviesDbContext context)
        {
            _context = context;
        }

        // GET: Movie
        public IActionResult Index(int page = 1, int size = 13)
        {
            return View( PagingListAsync<Movie>.Create(
                (p, s) => 
                    _context.Movies
                        .OrderBy(b => b.Title)
                        .Skip((p - 1) * s)
                        .Take(s)
                        .AsAsyncEnumerable(),
                _context.Movies.Count(),
                page,
                size));
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.MovieCasts) // Ładuj powiązanych aktorów
                .Include(m => m.Persons)   // Ładuj powiązane osoby
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movie/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        
        
        [HttpGet]
        public IActionResult AddActor(int? id)
        {
            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == id);
            if (movie == null)
            {
                return NotFound();
            }

            var model = new AddActorViewModel { MovieId = id.Value };
            return View(model);
        }

        
   

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActor(AddActorViewModel model)
        {
            if (model.MovieId == 0)
            {
                return NotFound();
            }

            var movie = await _context.Movies.Include(m => m.MovieCasts).FirstOrDefaultAsync(m => m.MovieId == model.MovieId);
            if (movie == null)
            {
                return NotFound();
            }
            // korzystamy sobie z _context.people bo ma to dostep do person z bazy danych
            var person = await _context.People.FirstOrDefaultAsync(p => p.PersonName == model.PersonName);
            if (person == null)
            {
                int? id = _context.People.Max(p=>p.PersonId);
                if (id == null)
                    id = 1;
                else
                    id++;
                
               
                // Tworzymy nową osobę
                person = new Person { PersonName = model.PersonName, PersonId = (int)id };
                _context.People.Add(person);  // Dodajemy nową osobę

                await _context.SaveChangesAsync(); // Zapisujemy ją, aby przypisać PersonId
            }

// Sprawdzamy, czy person.PersonId jest poprawnie ustawione
            if (person.PersonId == 0)
            {
                // Jeśli PersonId jest 0, oznacza to, że nie zostało przypisane
                return BadRequest("PersonId nie zostało przypisane.");
            }

// Tworzymy nowy rekord w MovieCast
            var movieCast = new MovieCast
            {
                MovieId = movie.MovieId,
                PersonId = person.PersonId, // Przypisujemy PersonId
                CharacterName = model.CharacterName,
                GenderId = model.GenderId,
                CastOrder = model.CastOrder
            };

            _context.MovieCasts.Add(movieCast);
            await _context.SaveChangesAsync();

           

            return RedirectToAction("Details", new { id = movie.MovieId });
        }




        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,Title,Budget,Homepage,Overview,Popularity,ReleaseDate,Revenue,Runtime,MovieStatus,Tagline,VoteAverage,VoteCount")] Movie movie)
        {
            if (id != movie.MovieId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
      

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }
    }
}
