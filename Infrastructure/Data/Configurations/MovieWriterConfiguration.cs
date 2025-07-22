
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MovieWriterConfiguration : IEntityTypeConfiguration<MovieWriter>
{
    public void Configure(EntityTypeBuilder<MovieWriter> builder)
    {
        builder.HasKey(mw => new { mw.MovieId, mw.WriterId });

        builder
            .HasOne(mw => mw.Movie)
            .WithMany(m => m.MovieWriters)
            .HasForeignKey(mw => mw.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(mw => mw.Writer)
            .WithMany(w => w.MovieWriters)
            .HasForeignKey(mw => mw.WriterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

