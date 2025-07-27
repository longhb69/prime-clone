using Application.Dtos;
using Application.Interfaces;
using Application.Movies.Commands.CreateMovie;
using Application.Movies.Commands.DeleteMovie;
using Application.Movies.Commands.UpdateMovie;
using Application.Movies.Queries.GetMovieByGenresWithPagination;
using Application.Movies.Queries.GetMovieById;
using Application.Movies.Queries.GetMovieByPerson;
using Application.Movies.Queries.GetMoviesWithPagination;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly ISender _sender;
    private readonly ILogger<MoviesController> _logger;

    public MoviesController(IMovieService movieService, ISender sender, ILogger<MoviesController> logger)
    {
        _movieService = movieService;
        _sender = sender;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var movies = await _sender.Send(new GetMoviesWithPaginationQuery(), cancellationToken);
        return Ok(movies);
    }

    [HttpGet("{id}", Name="GetMovieById")]
    public async Task<IActionResult> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        var movie = await _sender.Send(new GetMovieByIdQuery(id), cancellationToken);

        if (movie == null)
            return NotFound();

        return Ok(movie);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync(CreateMovieCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var movieId = await _sender.Send(command, cancellationToken);
            return CreatedAtRoute("GetMovieById", new { id = movieId }, command);
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
            await _sender.Send(new DeleteMovieCommand(id));
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
    public async Task<IActionResult> PatchMovie(string id, UpdateMovieCommand command , CancellationToken cancellationToken)
    {
        try
        {

            await _sender.Send(new UpdateMovieCommand(id), cancellationToken);
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
    [HttpGet("genre")]
    public async Task<IActionResult> GetByGenreAsync([FromQuery]int[] genreIds, CancellationToken cancellationToken)
    {
        var movies = await _sender.Send(new GetMoviesByGenresWithPaginationQuery(genreIds), cancellationToken);
        return Ok(movies);
    }
    [HttpGet("person")]
    public async Task<IActionResult> GetByPersonAsync(int[] personIds, CancellationToken cancellationToken)
    {
        var movies = await _sender.Send(new GetMoviesByPersonQuery(personIds), cancellationToken);
        return Ok(movies);
    }
    
}

