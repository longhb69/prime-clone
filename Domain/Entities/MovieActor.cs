using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class MovieActor
{
    public string MovieId { get; set; }
    public Movie Movie { get; set; }

    public int ActorId { get; set; }
    public Person Actor { get; set; }
}

