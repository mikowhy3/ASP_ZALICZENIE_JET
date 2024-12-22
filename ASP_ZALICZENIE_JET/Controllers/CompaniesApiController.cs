using ASP_ZALICZENIE_JET.Models.Movies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ASP_ZALICZENIE_JET.Controllers;

[ApiController]
[Route("/api/companies")]
public class CompaniesApiController:ControllerBase
{
    
    private MoviesDbContext _context;

    public CompaniesApiController(MoviesDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult GetFiltered(string fragment)
    {
        // zwracamy metode ok
        return Ok(
            _context.ProductionCompanies
                .Where(c=>c.CompanyName.ToLower().Contains(fragment.ToLower()))
                .OrderBy(c=>c.CompanyName)
                .AsNoTracking()
                .AsEnumerable()
        );
    }
}