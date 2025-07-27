using Application.Dtos;
using Application.Interfaces;

namespace Application.Movies.Queries.GetMovieByPerson;
public record GetMoviesByPersonQuery(int[] personIds) : IRequest<List<MovieDto>> { }


public class GetMoviesByPersonQueryHandler : IRequestHandler<GetMoviesByPersonQuery, List<MovieDto>>
{
    private readonly IApplicationDbContext _context;
    public GetMoviesByPersonQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<MovieDto>> Handle(GetMoviesByPersonQuery request, CancellationToken cancellationToken)
    {
        var movies = await _context.Movies
             .Where(m => m.MovieActors.Any(ma => request.personIds.Contains(ma.ActorId)) 
                    || m.MovieDirectors.Any(md => request.personIds.Contains(md.DirectorId))
                    || m.MovieWriters.Any(mw => request.personIds.Contains(mw.WriterId))
              )
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
}

