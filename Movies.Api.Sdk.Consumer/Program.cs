using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Sdk.Consumer;

static class Program
{
    private static readonly IServiceCollection Services = new ServiceCollection();

    static async Task Main(string[] args)
    {
        Services.ConfigureRefitClient();

        var provider = Services.BuildServiceProvider();

        var moviesApi = provider.GetRequiredService<IMoviesApi>();

        var movie = await GetMovieAsync(moviesApi);

        var movies = await GetMoviesAsync(moviesApi);

        var newMovie = await moviesApi.CreateMovieAsync(new CreateMovieRequest
        {
            Title = "Spiderman 2",
            YearOfRelease = 2002,
            Genres = new[] { "Action" }
        });

        await moviesApi.UpdateMovieAsync(newMovie.Id, new UpdateMovieRequest()
        {
            Title = "Spiderman 2",
            YearOfRelease = 2002,
            Genres = new[] { "Action", "Adventure" }
        });

        await moviesApi.DeleteMovieAsync(newMovie.Id);

        Console.WriteLine($"{JsonSerializer.Serialize(movie)}\n");

        foreach (var movieResponse in movies.Items)
        {
            Console.WriteLine($"{JsonSerializer.Serialize(movieResponse)}\n");
        }
    }


    private static async Task<MovieResponse> GetMovieAsync(IMoviesApi api)
    {
        return await api.GetMovieAsync("nick-the-greek-2-2024");
    }

    private static async Task<MoviesResponse> GetMoviesAsync(IMoviesApi api)
    {
        var request = new GetAllMoviesRequest()
        {
            Title = null,
            Year = null,
            SortBy = null,
            Page = 1,
            PageSize = 3
        };

        return await api.GetMoviesAsync(request);
    }
}