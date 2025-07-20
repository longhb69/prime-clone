using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;
public class Person
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
    public ICollection<MovieDirector> MovieDirectors { get; set; } = new List<MovieDirector>();
    public ICollection<MovieWriter> MovieWriters { get; set; } = new List<MovieWriter>();
}

