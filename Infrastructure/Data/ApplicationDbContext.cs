using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.Indentity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using System.Reflection;
using Application.Interfaces;

namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Person> Persons => Set<Person>();
    public DbSet<Language> Languages => Set<Language>();
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}