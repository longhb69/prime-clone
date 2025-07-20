using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Infrastructure.Indentity;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Person> Persons => Set<Person>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<MovieActor>()
            .HasKey(ma => new { ma.MovieId, ma.ActorId });

        builder.Entity<MovieActor>()
            .HasOne(ma => ma.Movie)
            .WithMany(m => m.MovieActors)
            .HasForeignKey(ma => ma.MovieId);

        builder.Entity<MovieActor>()
            .HasOne(ma => ma.Actor)
            .WithMany(p => p.MovieActors)
            .HasForeignKey(ma => ma.ActorId);


        builder.Entity<MovieDirector>()
            .HasKey(md => new { md.MovieId, md.DirectorId });

        builder.Entity<MovieDirector>()
            .HasOne(md => md.Movie)
            .WithMany(m => m.MovieDirectors)
            .HasForeignKey(md => md.MovieId);

        builder.Entity<MovieDirector>() 
            .HasOne(md => md.Director)
            .WithMany(d => d.MovieDirectors)
            .HasForeignKey(md => md.DirectorId);

        builder.Entity<MovieWriter>()
            .HasKey(mw => new { mw.MovieId, mw.WriterId });

        builder.Entity<MovieWriter>()
            .HasOne(mw => mw.Movie)
            .WithMany(m => m.MovieWriters)
            .HasForeignKey(mw => mw.MovieId);

        builder.Entity<MovieWriter>()
            .HasOne(mw => mw.Writer)
            .WithMany(w => w.MovieWriters)
            .HasForeignKey(mw => mw.WriterId);

        builder.Entity<Movie>()
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(j => j.ToTable("MovieGenres"));

        builder.Entity<Country>()
            .HasMany(c => c.Movies)
            .WithMany(m => m.Countries)
            .UsingEntity(j => j.ToTable("MovieCountry"));

        base.OnModelCreating(builder);
    }
}