using Domain.Enums;
namespace Application.Dtos;

public class CreateMovieDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public DateOnly Released { get; set; }
    public int Runtime { get; set; }
    public string Plot { get; set; }
    public List<int> LanguageIds { get; set; }
    public string Poster { get; set; }
    public List<int> GenreIds { get; set; }
    public List<int> CountryIds { get; set; }
    public List<int> ActorIds { get; set; }
    public List<int> DirectorIds { get; set; }
    public List<int> WriterIds { get; set; }
    public MovieType Type { get; set; }
    public int TotalSeasons { get; set; }
    public double ImdbScore { get; set; }
    public int MetacriticScore { get; set; }
    public int RottenTomatoesScore { get; set; }
}