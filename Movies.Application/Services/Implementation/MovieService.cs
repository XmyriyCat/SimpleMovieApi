using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Services.Contracts;

namespace Movies.Application.Services.Implementation;

public class MovieService : IMovieService
{
    private readonly Repositories.IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _movieValidator;

    public MovieService(Repositories.IMovieRepository movieRepository, IValidator<Movie> movieValidator)
    {
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
    }

    public async Task<bool> CreateAsync(Movie movie)
    {
        await _movieValidator.ValidateAndThrowAsync(movie);
        
        return await _movieRepository.CreateAsync(movie);
    }

    public Task<Movie?> GetByIdAsync(Guid id)
    {
        return _movieRepository.GetByIdAsync(id);
    }

    public Task<Movie?> GetBySlugAsync(string slug)
    {
        return _movieRepository.GetBySlugAsync(slug);
    }

    public Task<IEnumerable<Movie>> GetAllAsync()
    {
        return _movieRepository.GetAllAsync();
    }

    public async Task<Movie?> UpdateAsync(Movie movie)
    {
        await _movieValidator.ValidateAndThrowAsync(movie);
        
        var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id);
        
        if (!movieExists)
        {
            return null;
        }

        await _movieRepository.UpdateAsync(movie);

        return movie;
    }

    public Task<bool> DeleteByIdAsync(Guid id)
    {
        return _movieRepository.DeleteByIdAsync(id);
    }
}