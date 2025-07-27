using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Movies.Queries.GetMovieByGenresWithPagination;

public record GetMoviesByGenresWithPaginationQuery(int[] genreIds) : IRequest<List<MovieDto>> { }

public class GetMoviesByGenresWithPaginationQueryHandler : IRequestHandler<GetMoviesByGenresWithPaginationQuery, List<MovieDto>>
{
    private readonly IApplicationDbContext _context;
    public GetMoviesByGenresWithPaginationQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<List<MovieDto>> Handle(GetMoviesByGenresWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var movies = await _context.Movies
            .Where(m => m.Genres.Any(g => request.genreIds.Contains(g.Id)))
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

