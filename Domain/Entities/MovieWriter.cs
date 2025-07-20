namespace Domain.Entities;
public class MovieWriter
{
    public string MovieId { get; set; }
    public Movie Movie { get; set; }

    public int WriterId { get; set; }
    public Person Writer { get; set; }
}
