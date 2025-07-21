using Domain.Enums;

namespace Domain.Entities;
public class Movie
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public DateOnly Released { get; set; }
    public int Runtime { get; set; }
    public string Plot { get; set; }
    public string Poster { get; set; }
    public ICollection<Genre> Genres { get; set; } = new List<Genre>();
    public ICollection<Country> Countries { get; set; } = new List<Country>();

    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    public ICollection<MovieDirector> MovieDirectors { get; set; } = new List<MovieDirector>();
    public ICollection<MovieWriter> MovieWriters { get; set; } = new List<MovieWriter>();

    public MovieType Type { get; set; }
    public int TotalSeasons { get; set; }
    public double ImdbScore { get; set; }      
    public int MetacriticScore { get; set; }   
    public int RottenTomatoesScore { get; set; } 
}