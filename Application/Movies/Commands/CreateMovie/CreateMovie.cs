using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;


namespace Application.Movies.Commands.CreateMovie;

public record CreateMovieCommand : IRequest<string>
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public DateOnly Released { get; set; }
    public int Runtime { get; set; }
    public string Plot { get; set; }
    public List<int> LanguageIds { get; set; } = new();
    public string Poster { get; set; }
    public List<int> GenreIds { get; set; } = new();
    public List<int> CountryIds { get; set; } = new();
    public List<int> ActorIds { get; set; } = new();
    public List<int> DirectorIds { get; set; } = new();
    public List<int> WriterIds { get; set; } = new();
    public MovieType Type { get; set; }
    public int TotalSeasons { get; set; }
    public double ImdbScore { get; set; }
    public int MetacriticScore { get; set; }
    public int RottenTomatoesScore { get; set; }
}

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, string>
{
    public readonly IApplicationDbContext _context;

    public CreateMovieCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = new Movie
        {
            Id = request.Id,
            Title = request.Title,
            Year = request.Year,
            Released = request.Released,
            Runtime = request.Runtime,
            Plot = request.Plot,
            Poster = request.Poster,
            Type = request.Type,
            TotalSeasons = request.TotalSeasons,
            ImdbScore = request.ImdbScore,
            MetacriticScore = request.MetacriticScore,
            RottenTomatoesScore = request.RottenTomatoesScore,

            Genres = await _context.Genres.Where(g => request.GenreIds.Contains(g.Id)).ToListAsync(cancellationToken),
            Countries = await _context.Countries.Where(c => request.CountryIds.Contains(c.Id)).ToListAsync(cancellationToken)
        };

        //relationship field
        var validLanguageIds = await _context.Languages
            .Where(l => request.LanguageIds.Contains(l.Id))
            .Select(l => l.Id)
            .ToListAsync(cancellationToken);

        movie.MovieLanguages = validLanguageIds.Select(id => new MovieLanguage
        {
            MovieId = request.Id,
            LanguageId = id
        }).ToList();

        var validActorIds = await _context.Persons
            .Where(p => request.ActorIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

        movie.MovieActors = validActorIds.Select(id => new MovieActor
        {
            MovieId = request.Id,
            ActorId = id
        }).ToList();

        var validWriterIds = await _context.Persons
            .Where(p => request.WriterIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

        movie.MovieWriters = validWriterIds.Select(id => new MovieWriter()
        {
            MovieId = request.Id,
            WriterId = id
        }).ToList();

        var validDirectorIds = await _context.Persons
            .Where(p => request.DirectorIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(cancellationToken);

        movie.MovieDirectors = validDirectorIds.Select(id => new MovieDirector
        {
            MovieId = request.Id,
            DirectorId = id
        }).ToList();

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync(cancellationToken);

        return movie.Id;
    }
}

