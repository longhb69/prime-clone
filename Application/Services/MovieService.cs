using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;

    public MovieService(IMovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<Movie>> GetAllAsync() => await _repository.GetAllAsync();
    public async Task<Movie?> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);
}       

