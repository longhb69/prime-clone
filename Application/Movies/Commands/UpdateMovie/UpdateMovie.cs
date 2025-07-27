using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;


namespace Application.Movies.Commands.UpdateMovie;
public record UpdateMovieCommand(string id) : IRequest
{
    public string? Title { get; set; }
    public int? Year { get; set; }
    public DateOnly? Released { get; set; }
    public int? Runtime { get; set; }
    public string? Plot { get; set; }
    public string? Poster { get; set; }
    public MovieType? Type { get; set; }
    public int? TotalSeasons { get; set; }
    public double? ImdbScore { get; set; }
    public int? MetacriticScore { get; set; }
    public int? RottenTomatoesScore { get; set; }

    public List<int>? GenreIds { get; set; }
    public List<int>? CountryIds { get; set; }
    public List<int>? LanguageIds { get; set; }
    public List<int>? ActorIds { get; set; }
    public List<int>? DirectorIds { get; set; }
    public List<int>? WriterIds { get; set; }
}
public class UpdateMovieHandler : IRequestHandler<UpdateMovieCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateMovieHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _context.Movies.FindAsync(request.id, cancellationToken);

        if (movie == null)
            throw new Exception("movie not found");

        //basic field
        if (request.Title != null) movie.Title = request.Title;
        if (request.Year.HasValue) movie.Year = request.Year.Value;
        if (request.Released.HasValue) movie.Released = request.Released.Value;
        if (request.Runtime.HasValue) movie.Runtime = request.Runtime.Value;
        if (request.Plot != null) movie.Plot = request.Plot;
        if (request.Poster != null) movie.Poster = request.Poster;
        if (request.Type.HasValue) movie.Type = request.Type.Value;
        if (request.TotalSeasons.HasValue) movie.TotalSeasons = request.TotalSeasons.Value;
        if (request.ImdbScore.HasValue) movie.ImdbScore = request.ImdbScore.Value;
        if (request.MetacriticScore.HasValue) movie.MetacriticScore = request.MetacriticScore.Value;
        if (request.RottenTomatoesScore.HasValue) movie.RottenTomatoesScore = request.RottenTomatoesScore.Value;

        //relation field
        if (request.GenreIds != null)
            movie.Genres = await _context.Genres.Where(g => request.GenreIds.Contains(g.Id)).ToListAsync(cancellationToken);
        if (request.CountryIds != null)
            movie.Countries = await _context.Countries.Where(c => request.CountryIds.Contains(c.Id)).ToListAsync(cancellationToken);

        if (request.ActorIds != null)
        {
            var validActorIds = await _context.Persons
                .Where(p => request.ActorIds.Contains(p.Id))
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

        if (request.DirectorIds != null)
        {
            var validDirectorIds = await _context.Persons
                .Where(p => request.DirectorIds.Contains(p.Id))
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

        if (request.WriterIds != null)
        {
            var validWriterIds = await _context.Persons
                .Where(p => request.WriterIds.Contains(p.Id))
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

        if (request.LanguageIds != null)
        {
            var validLanguageIds = await _context.Languages
                .Where(l => request.LanguageIds.Contains(l.Id))
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

    public void UpdateMovieRelations<TJoin>(
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

