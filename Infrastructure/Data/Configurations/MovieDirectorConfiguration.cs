using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MovieDirectorConfiguration : IEntityTypeConfiguration<MovieDirector>
{
    public void Configure(EntityTypeBuilder<MovieDirector> builder)
    {
        builder.HasKey(md => new { md.MovieId, md.DirectorId });

        builder
            .HasOne(md => md.Movie)
            .WithMany(m => m.MovieDirectors)
            .HasForeignKey(md => md.MovieId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(md => md.Director)
            .WithMany(d => d.MovieDirectors)
            .HasForeignKey(md => md.DirectorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
