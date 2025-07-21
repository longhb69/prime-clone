using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _context;

    public MovieRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MovieDto>> GetAllAsync()
    {
        var movies = await _context.Movies
            .Include(m => m.Genres)
            .Include(m => m.MovieDirectors)
                .ThenInclude(m => m.Director)
            .Include(m => m.MovieWriters)
                .ThenInclude(m => m.Writer)
            .Include(m => m.MovieActors)
                .ThenInclude(m  => m.Actor)
            .Include(m => m.Countries)
            .ToListAsync();

        var movieDtos = movies.Select(movie => new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Released = movie.Released,
            Runtime = movie.Runtime,
            Plot = movie.Plot,
            Poster = movie.Poster,
            Genres = movie.Genres.Select(g => 
                new GenreDto{ Id = g.Id, Name = g.Name }
                ).ToList(),
            Director = movie.MovieDirectors.Select(md => 
                new PersonDto {Id = md.Director.Id, FullName = md.Director.FullName}
                ).ToList(),
            Writer = movie.MovieWriters.Select(mw => 
                new PersonDto {Id = mw.Writer.Id, FullName = mw.Writer.FullName}
                ).ToList(),
            Actors = movie.MovieActors.Select(ma => 
                new PersonDto {Id = ma.Actor.Id, FullName = ma.Actor.FullName}
            ).ToList(),    
            Country = movie.Countries.Select(c => 
                new CountryDto { Id= c.Id, Name = c.Name }
                ).ToList(),
            Type = movie.Type,
            TotalSeasons = movie.TotalSeasons,
            ImdbScore = movie.ImdbScore,
            MetacriticScore = movie.MetacriticScore,
            RottenTomatoesScore = movie.RottenTomatoesScore
        }).ToList();
        return movieDtos;
    }

    public async Task<MovieDto?> GetByIdAsync(string id)
    {
        var movie = await _context.Movies
            .Include(m => m.Genres)
            .Include(m => m.MovieDirectors)
                .ThenInclude(m => m.Director)
            .Include(m => m.MovieWriters)
                .ThenInclude(m => m.Writer)
            .Include(m => m.MovieActors)
                .ThenInclude(m  => m.Actor)
            .Include(m => m.Countries)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return null;

        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Released = movie.Released,
            Runtime = movie.Runtime,
            Plot = movie.Plot,
            Poster = movie.Poster,
            Genres = movie.Genres.Select(g => 
                new GenreDto{ Id = g.Id, Name = g.Name }
            ).ToList(),
            Director = movie.MovieDirectors.Select(md => 
                new PersonDto {Id = md.Director.Id, FullName = md.Director.FullName}
            ).ToList(),
            Writer = movie.MovieWriters.Select(mw => 
                new PersonDto {Id = mw.Writer.Id, FullName = mw.Writer.FullName}
            ).ToList(),
            Actors = movie.MovieActors.Select(ma => 
                new PersonDto {Id = ma.Actor.Id, FullName = ma.Actor.FullName}
            ).ToList(),   
            Country = movie.Countries.Select(c => 
                new CountryDto { Id= c.Id, Name = c.Name }
            ).ToList(),
            Type = movie.Type,
            TotalSeasons = movie.TotalSeasons,
            ImdbScore = movie.ImdbScore,
            MetacriticScore = movie.MetacriticScore,
            RottenTomatoesScore = movie.RottenTomatoesScore
        };
    }

    public async Task<List<MovieDto>> GetByGenreAsync(int genreId)
    {
        var movies = await _context.Movies
            .Where(m => m.Genres.Any(g => g.Id == genreId))
            .Include(m => m.Genres)
            .Include(m => m.MovieDirectors)
            .ThenInclude(m => m.Director)
            .Include(m => m.MovieWriters)
            .ThenInclude(m => m.Writer)
            .Include(m => m.MovieActors)
            .ThenInclude(m => m.Actor)
            .Include(m => m.Countries)
            .ToListAsync();
        
        var movieDtos = movies.Select(movie => new MovieDto
        {   
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Released = movie.Released,
            Runtime = movie.Runtime,
            Plot = movie.Plot,
            Poster = movie.Poster,
            Genres = movie.Genres.Select(g => 
                new GenreDto{ Id = g.Id, Name = g.Name }
            ).ToList(),
            Director = movie.MovieDirectors.Select(md => 
                new PersonDto {Id = md.Director.Id, FullName = md.Director.FullName}
            ).ToList(),
            Writer = movie.MovieWriters.Select(mw => 
                new PersonDto {Id = mw.Writer.Id, FullName = mw.Writer.FullName}
            ).ToList(),
            Actors = movie.MovieActors.Select(ma => 
                new PersonDto {Id = ma.Actor.Id, FullName = ma.Actor.FullName}
            ).ToList(),    
            Country = movie.Countries.Select(c => 
                new CountryDto { Id= c.Id, Name = c.Name }
            ).ToList(),
            Type = movie.Type,
            TotalSeasons = movie.TotalSeasons,
            ImdbScore = movie.ImdbScore,
            MetacriticScore = movie.MetacriticScore,
            RottenTomatoesScore = movie.RottenTomatoesScore
        }).ToList();

        return movieDtos;
    }

    public async Task AddAsync(Movie movie)
    {
        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Movie movie)
    {
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var moive = await _context.Movies.FindAsync(id);
        if (moive != null)
        {
            _context.Remove(moive);
            await _context.SaveChangesAsync();
        }
    }
}

