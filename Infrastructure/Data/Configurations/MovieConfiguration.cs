using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configurations;

public class MovieConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.Property(m => m.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder
            .HasMany(m => m.Genres)
            .WithMany(g => g.Movies)
            .UsingEntity(j => j.ToTable("MovieGenres"));

        builder
            .HasMany(m => m.Countries)
            .WithMany(c => c.Movies)
            .UsingEntity(j => j.ToTable("MovieCountry"));

        builder
            .HasMany(m => m.MovieActors)
            .WithOne(ma => ma.Movie)
            .HasForeignKey(ma => ma.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(m => m.MovieDirectors)
            .WithOne(md => md.Movie)
            .HasForeignKey(md => md.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(m => m.MovieWriters)
            .WithOne(mw => mw.Movie)
            .HasForeignKey(mw => mw.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}

