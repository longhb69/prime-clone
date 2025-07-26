using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly ILogger<MoviesController> _logger;

    public MoviesController(IMovieService movieService, ILogger<MoviesController> logger)
    {
        _movieService = movieService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        try
        {
            var movies = await _movieService.GetAllAsync(cancellationToken);
            return Ok(movies);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Request was cancelled by the client");
            return StatusCode(499, "Client Closed Request");    
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "Error occurred while fetching movies");
            return StatusCode(500, "An error occurred while fetching movies.");
        }
    }

    [HttpGet("{id}", Name="GetMovieById")]
    public async Task<IActionResult> GetAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var movie = await _movieService.GetByIdAsync(id, cancellationToken);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }
        catch (OperationCanceledException)
        {
            _logger.LogWarning("Request was cancelled by the client");
            return StatusCode(499, "Client Closed Request");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching movies.");
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody]CreateMovieDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _movieService.AddAsync(dto, cancellationToken);
            return CreatedAtRoute("GetMovieById", new { id = dto.Id }, dto);
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new
            {
                error = "Database error occurred while saving movie.",
                message = ex.InnerException?.Message ?? ex.Message
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _movieService.DeleteAsync(id, cancellationToken);

            if (!result)
                return NotFound();

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new
            {
                error = "Database error occurred while saving movie.",
                message = ex.InnerException?.Message ?? ex.Message
            });
        }
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> PatchMovie(string id, [FromBody] PatchMovieDto dto, CancellationToken cancellationToken)
    {
        try
        {
            await _movieService.UpdateAsync(id, dto, cancellationToken);
            return Ok();
        }
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new
            {
                error = "Database error occurred while saving movie.",
                message = ex.InnerException?.Message ?? ex.Message
            });
        }
    }
    [HttpGet("genre")]
    public async Task<IActionResult> GetByGenreAsync([FromQuery] int[] genreId, CancellationToken cancellationToken)
    {
        var movies = await _movieService.GetByGenreAsync(genreId, cancellationToken);
        return Ok(movies);
    }
}

