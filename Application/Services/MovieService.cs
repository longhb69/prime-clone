using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dtos;

namespace Application.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _repository;

    public MovieService(IMovieRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<MovieDto>> GetAllAsync() => await _repository.GetAllAsync();
    public async Task<MovieDto?> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);
    public async Task<List<MovieDto>> GetByGenreAsync(int genreId) => await _repository.GetByGenreAsync(genreId);

    public async Task UpdateAsync(string id, UpdateMovieDto dto)
    {
        var movie = await _repository.GetByIdAsync(id);
        
        if(movie == null) 
            throw new Exception("movie not found");
        
        //basic field
        movie.Title = movie.Title;
        movie.Year = movie.Year;
        movie.Released = movie.Released;
        movie.Plot = movie.Plot;
        movie.Poster = movie.Poster;
        movie.Type = movie.Type;
        movie.TotalSeasons = movie.TotalSeasons;
        movie.ImdbScore = movie.ImdbScore;
        movie.MetacriticScore = movie.MetacriticScore;
        movie.RottenTomatoesScore = movie.RottenTomatoesScore;
        
        //handle genre
    }

    public Task AddAsync(Movie movie)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }
}       

