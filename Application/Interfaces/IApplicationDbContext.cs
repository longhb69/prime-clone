using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Movie> Movies { get; }
    DbSet<Genre> Genres { get; }
    DbSet<Country> Countries { get; }
    DbSet<Person> Persons { get; }
    DbSet<Language> Languages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

