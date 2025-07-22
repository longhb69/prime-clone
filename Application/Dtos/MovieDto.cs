using Domain.Entities;
using Domain.Enums;
namespace Application.Dtos;
using System.Text.Json.Serialization;

public class MovieDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public DateOnly Released { get; set; }
    public int Runtime { get; set; }
    public string Plot { get; set; }
    public List<LanguageDto> Languages { get; set; }
    public string Poster { get; set; }
    public List<GenreDto> Genres { get; set; }
    public List<PersonDto> Director { get; set; }
    public List<PersonDto> Writer { get; set; }
    public List<PersonDto> Actors  { get; set; }
    public List<CountryDto> Country  { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public MovieType Type { get; set; }
    public int TotalSeasons { get; set; }
    public double ImdbScore { get; set; }      
    public int MetacriticScore { get; set; }   
    public int RottenTomatoesScore { get; set; } 
}