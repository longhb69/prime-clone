using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces;

public interface IMovieRepository
{
    Task<List<MovieDto>> GetAllAsync();
    Task<MovieDto?> GetByIdAsync(string id);
    Task<List<MovieDto>> GetByGenreAsync(int genreId);
    Task AddAsync(Movie movie);
    Task UpdateAsync(Movie movie);
    Task DeleteAsync(string id);
}


