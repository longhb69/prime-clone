using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;
using Microsoft.Net.Http.Headers;

namespace Application.Interfaces;

public interface IMovieService
{
    Task<List<MovieDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<MovieDto?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<List<MovieDto>> GetByGenreAsync(int[] genreId, CancellationToken cancellationToken);
    Task UpdateAsync(string id, PatchMovieDto dto, CancellationToken cancellationToken);
    Task AddAsync(CreateMovieDto dto, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);   
}

