using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class MovieService : IMovieService
{
    private readonly IApplicationDbContext _context;
    public MovieService(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<MovieDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var movies = await _context.Movies
            .Include(m => m.Genres)
            .Include(m => m.MovieDirectors)
                .ThenInclude(m => m.Director)
            .Include(m => m.MovieWriters)
                .ThenInclude(m => m.Writer)
            .Include(m => m.MovieActors)
                .ThenInclude(m => m.Actor)
            .Include(m => m.Countries)
            .Include(m => m.MovieLanguages)
                .ThenInclude(ml => ml.Language)
            .ToListAsync(cancellationToken);

        //for long running loop
        //cancellationToken.ThrowIfCancellationRequested();
        var movieDtos = movies.Select(movie => new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Released = movie.Released,
            Runtime = movie.Runtime,
            Plot = movie.Plot,
            Languages = movie.MovieLanguages.Select(l =>
                new LanguageDto { Id = l.Language.Id, Name = l.Language.Name }
            ).ToList(),
            Poster = movie.Poster,
            Genres = movie.Genres.Select(g =>
                new GenreDto { Id = g.Id, Name = g.Name }
                ).ToList(),
            Director = movie.MovieDirectors.Select(md =>
                new PersonDto { Id = md.Director.Id, FullName = md.Director.FullName }
                ).ToList(),
            Writer = movie.MovieWriters.Select(mw =>
                new PersonDto { Id = mw.Writer.Id, FullName = mw.Writer.FullName }
                ).ToList(),
            Actors = movie.MovieActors.Select(ma =>
                new PersonDto { Id = ma.Actor.Id, FullName = ma.Actor.FullName }
            ).ToList(),
            Country = movie.Countries.Select(c =>
                new CountryDto { Id = c.Id, Name = c.Name }
                ).ToList(),
            Type = movie.Type,
            TotalSeasons = movie.TotalSeasons,
            ImdbScore = movie.ImdbScore,
            MetacriticScore = movie.MetacriticScore,
            RottenTomatoesScore = movie.RottenTomatoesScore
        }).ToList();
        return movieDtos;
    }
    public async Task<MovieDto?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var movie = await _context.Movies
            .Include(m => m.Genres)
            .Include(m => m.MovieDirectors)
                .ThenInclude(m => m.Director)
            .Include(m => m.MovieWriters)
                .ThenInclude(m => m.Writer)
            .Include(m => m.MovieActors)
                .ThenInclude(m => m.Actor)
            .Include(m => m.Countries)
            .Include(m => m.MovieLanguages)
                .ThenInclude(ml => ml.Language)
            .FirstOrDefaultAsync((m => m.Id == id), cancellationToken);

        if (movie == null) return null;

        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Released = movie.Released,
            Runtime = movie.Runtime,
            Plot = movie.Plot,
            Languages = movie.MovieLanguages.Select(l =>
                new LanguageDto { Id = l.Language.Id, Name = l.Language.Name }
            ).ToList(),
            Poster = movie.Poster,
            Genres = movie.Genres.Select(g =>
                new GenreDto { Id = g.Id, Name = g.Name }
            ).ToList(),
            Director = movie.MovieDirectors.Select(md =>
                new PersonDto { Id = md.Director.Id, FullName = md.Director.FullName }
            ).ToList(),
            Writer = movie.MovieWriters.Select(mw =>
                new PersonDto { Id = mw.Writer.Id, FullName = mw.Writer.FullName }
            ).ToList(),
            Actors = movie.MovieActors.Select(ma =>
                new PersonDto { Id = ma.Actor.Id, FullName = ma.Actor.FullName }
            ).ToList(),
            Country = movie.Countries.Select(c =>
                new CountryDto { Id = c.Id, Name = c.Name }
            ).ToList(),
            Type = movie.Type,
            TotalSeasons = movie.TotalSeasons,
            ImdbScore = movie.ImdbScore,
            MetacriticScore = movie.MetacriticScore,
            RottenTomatoesScore = movie.RottenTomatoesScore
        };
    }
    public async Task<List<MovieDto>> GetByGenreAsync(int[] genreId, CancellationToken cancellationToken = default)
    {
        var movies = await _context.Movies
            .Where(m => m.Genres.Any(g => genreId.Contains(g.Id)))
            .Include(m => m.Genres)
            .Include(m => m.MovieDirectors)
                .ThenInclude(m => m.Director)
            .Include(m => m.MovieWriters)
                .ThenInclude(m => m.Writer)
            .Include(m => m.MovieActors)
                .ThenInclude(m => m.Actor)
            .Include(m => m.Countries)
            .Include(m => m.MovieLanguages)
                .ThenInclude(ml => ml.Language)
            .ToListAsync(cancellationToken);

        var movieDtos = movies.Select(movie => new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Year = movie.Year,
            Released = movie.Released,
            Runtime = movie.Runtime,
            Plot = movie.Plot,
            Languages = movie.MovieLanguages.Select(l =>
                new LanguageDto { Id = l.Language.Id, Name = l.Language.Name }
            ).ToList(),
            Poster = movie.Poster,
            Genres = movie.Genres.Select(g =>
                new GenreDto { Id = g.Id, Name = g.Name }
            ).ToList(),
            Director = movie.MovieDirectors.Select(md =>
                new PersonDto { Id = md.Director.Id, FullName = md.Director.FullName }
            ).ToList(),
            Writer = movie.MovieWriters.Select(mw =>
                new PersonDto { Id = mw.Writer.Id, FullName = mw.Writer.FullName }
            ).ToList(),
            Actors = movie.MovieActors.Select(ma =>
                new PersonDto { Id = ma.Actor.Id, FullName = ma.Actor.FullName }
            ).ToList(),
            Country = movie.Countries.Select(c =>
                new CountryDto { Id = c.Id, Name = c.Name }
            ).ToList(),
            Type = movie.Type,
            TotalSeasons = movie.TotalSeasons,
            ImdbScore = movie.ImdbScore,
            MetacriticScore = movie.MetacriticScore,
            RottenTomatoesScore = movie.RottenTomatoesScore
        }).ToList();

        return movieDtos;
    }
    public async Task UpdateAsync(string id, PatchMovieDto dto, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies.FindAsync(id, cancellationToken);
        
        if(movie == null) 
            throw new Exception("movie not found");
        
        //basic field
        if (dto.Title != null) movie.Title = dto.Title;
        if (dto.Year.HasValue) movie.Year = dto.Year.Value;
        if (dto.Released.HasValue) movie.Released = dto.Released.Value;
        if (dto.Runtime.HasValue) movie.Runtime = dto.Runtime.Value;
        if (dto.Plot != null) movie.Plot = dto.Plot;
        if (dto.Poster != null) movie.Poster = dto.Poster;
        if (dto.Type.HasValue) movie.Type = dto.Type.Value;
        if (dto.TotalSeasons.HasValue) movie.TotalSeasons = dto.TotalSeasons.Value;
        if (dto.ImdbScore.HasValue) movie.ImdbScore = dto.ImdbScore.Value;
        if (dto.MetacriticScore.HasValue) movie.MetacriticScore = dto.MetacriticScore.Value;
        if (dto.RottenTomatoesScore.HasValue) movie.RottenTomatoesScore = dto.RottenTomatoesScore.Value;
        
        //relation field
        if(dto.GenreIds != null)
            movie.Genres =  await _context.Genres.Where(g => dto.GenreIds.Contains(g.Id)).ToListAsync(cancellationToken);
        if(dto.CountryIds != null)
            movie.Countries = await _context.Countries.Where(c => dto.CountryIds.Contains(c.Id)).ToListAsync(cancellationToken);

        if (dto.ActorIds != null)
        {
            var validActorIds = await _context.Persons
                .Where(p => dto.ActorIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            UpdateMovieRelations(
                movie.MovieActors,
                validActorIds,
                id => new MovieActor
                {
                    ActorId = id,
                    MovieId = movie.Id,
                }
                );
        }

        if (dto.DirectorIds != null)
        {
            var validDirectorIds = await _context.Persons
                .Where(p => dto.DirectorIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);
            
            UpdateMovieRelations(
                movie.MovieDirectors,
                validDirectorIds,
                id => new MovieDirector
                {
                    DirectorId = id,
                    MovieId = movie.Id,
                }
            );
        }
        
        if (dto.WriterIds != null)
        {
            var validWriterIds = await _context.Persons
                .Where(p => dto.WriterIds.Contains(p.Id))
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);
            
            UpdateMovieRelations(
                movie.MovieWriters,
                validWriterIds,
                id => new MovieWriter
                {
                    WriterId = id,
                    MovieId = movie.Id,
                }
            );
        }
        
        if (dto.LanguageIds != null)
        {
            var validLanguageIds = await _context.Languages
                .Where(l => dto.LanguageIds.Contains(l.Id))
                .Select(l => l.Id)
                .ToListAsync(cancellationToken);
            
            UpdateMovieRelations(
                movie.MovieLanguages,
                validLanguageIds,
                id => new MovieLanguage
                {
                    LanguageId = id,
                    MovieId = movie.Id,
                }
            );
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync(CreateMovieDto dto, CancellationToken cancellationToken = default)
    {
        //base field
        var movie = new Movie
        {
            Id = dto.Id,
            Title = dto.Title,
            Year = dto.Year,
            Released = dto.Released,
            Runtime = dto.Runtime,
            Plot = dto.Plot,
            Poster = dto.Poster,
            Type = dto.Type,
            TotalSeasons = dto.TotalSeasons,
            ImdbScore = dto.ImdbScore,
            MetacriticScore = dto.MetacriticScore,
            RottenTomatoesScore = dto.RottenTomatoesScore,
            
            Genres = await _context.Genres.Where(g => dto.GenreIds.Contains(g.Id)).ToListAsync(cancellationToken),
            Countries = await _context.Countries.Where(c => dto.CountryIds.Contains(c.Id)).ToListAsync(cancellationToken)
        };

        //relationship field
        var validLanguageIds = await _context.Languages
            .Where(l => dto.LanguageIds.Contains(l.Id))
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        movie.MovieLanguages = validLanguageIds.Select(id => new MovieLanguage
        {
            MovieId = dto.Id,
            LanguageId = id
        }).ToList();
        
        var validActorIds = await _context.Persons
            .Where(p => dto.ActorIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);
        
        movie.MovieActors = validActorIds.Select(id => new MovieActor
        {
            MovieId = dto.Id,
            ActorId = id
        }).ToList();
        
        var validWriterIds = await _context.Persons
            .Where(p => dto.WriterIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);
        
        movie.MovieWriters = validWriterIds.Select(id => new MovieWriter()
        {
            MovieId = dto.Id,
            WriterId = id
        }).ToList();
        
        var validDirectorIds = await _context.Persons
            .Where(p => dto.DirectorIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);
        
        movie.MovieDirectors = validDirectorIds.Select(id => new MovieDirector
        {
            MovieId = dto.Id,
            DirectorId = id
        }).ToList();

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var moive = await _context.Movies.FindAsync(id, cancellationToken);
        if (moive != null)
        {
            _context.Movies.Remove(moive);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        return false;
    }

    private void UpdateMovieRelations<TJoin>(
        ICollection<TJoin> collection,
        IEnumerable<int> ids,
        Func<int, TJoin> createJoinEntity)
    {
        //f you don’t call .Clear(), EF Core will think you're adding new links without removing the old ones.
        //You’ll end up with duplicates in the MovieActor table
        //or foreign key constraint errors (e.g., duplicate primary key in join table).
        collection.Clear();
        foreach (var id in ids)
        {
            collection.Add(createJoinEntity(id));
        }
    }
}       

