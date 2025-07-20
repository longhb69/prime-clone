using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class MovieDirector
{
    public string MovieId { get; set; }
    public Movie Movie { get; set; }

    public int DirectorId { get; set; }
    public Person Director { get; set; }
}
