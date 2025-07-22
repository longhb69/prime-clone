using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Interfaces;

public interface IMovieService
{
    Task<List<MovieDto>> GetAllAsync();
    Task<MovieDto?> GetByIdAsync(string id);
    Task<List<MovieDto>> GetByGenreAsync(int[] genreId);
    Task UpdateAsync(string id, UpdateMovieDto dto);
    Task AddAsync(Movie movie);
    Task DeleteAsync(string id);   
}

