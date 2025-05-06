using Movies.Application.Models;

namespace Movies.Application.Services.Contracts;

public interface IMovieService
{
    Task<bool> CreateAsync(Movie movie, CancellationToken token = default);
    
    Task<Movie?> GetByIdAsync(Guid id, Guid? userId = null, CancellationToken token = default);
    
    Task<Movie?> GetBySlugAsync(string slug, Guid? userId = null, CancellationToken token = default);

    Task<IEnumerable<Movie>> GetAllAsync(GetAllMoviesOptions options, CancellationToken token = default);
    
    Task<Movie?> UpdateAsync(Movie movie, Guid? userId = null, CancellationToken token = default);
    
    Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default);
}