using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Application.Interfaces;
using Ardalis.GuardClauses;


namespace Application.Movies.Queries.GetMovieById;

public record GetMovieByIdQuery(string Id) : IRequest<MovieDto>;

public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieDto>
{
    private readonly IApplicationDbContext _context;

    public GetMovieByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MovieDto> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
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
           .FirstOrDefaultAsync((m => m.Id == request.Id), cancellationToken);

        if (movie == null)
            throw new NotFoundException(request.Id ,$"User with id {request.Id} was not found.");

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
}
