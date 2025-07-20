using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesController(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies);
        }
        catch (Exception ex) 
        {
            return StatusCode(500, "An error occurred while fetching movies.");
        }
    }
}

