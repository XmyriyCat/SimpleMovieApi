using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

namespace Movies.Application.Repositories;

public class RatingRepository : IRatingRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public RatingRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> RateMovieAsync(Guid movieId, int rating, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var result = await connection.ExecuteAsync(
            new CommandDefinition("""
                                  insert into ratings(userid, movieid, rating)
                                  values (@userId, @movieId, @rating)
                                  on conflict (userid, movieid) do update
                                      set rating = @rating
                                  """, new { userId = userId, movieId = movieId, rating = rating },
                cancellationToken: token));

        return result > 0;
    }

    public async Task<float?> GetRatingAsync(Guid movieId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.QueryFirstOrDefaultAsync<float?>(
            new CommandDefinition("""
                                              select round(avg(r.rating), 1) from ratings r 
                                              where movieId = @movieId
                                  """, new { movieId = movieId }, cancellationToken: token));
    }

    public async Task<(float? Rating, int? UserRating)> GetRatingAsync(Guid movieId, Guid userId,
        CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.QueryFirstOrDefaultAsync<(float?, int?)>(
            new CommandDefinition("""
                                  select round(avg(rating), 1),
                                      (select rating 
                                       from ratings
                                       where movieId = @movieId 
                                         and userId = @userId
                                       limit 1)
                                  from ratings
                                  where movieId = @movieId
                                  """, new { movieId = movieId, userId = userId }, cancellationToken: token));
    }

    public async Task<bool> DeleteRatingAsync(Guid movieId, Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        var result = await connection.ExecuteAsync(
            new CommandDefinition("""
                                  delete from ratings
                                  where movieId = @movieId 
                                    and userId = @userId
                                  """, new { movieId = movieId, userId = userId }, cancellationToken: token));

        return result > 0;
    }

    public async Task<IEnumerable<MovieRating>> GetRatingsForUserAsync(Guid userId, CancellationToken token = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(token);

        return await connection.QueryAsync<MovieRating>(
            new CommandDefinition("""
                                  select r.movieId, r.rating, m.slug 
                                  from ratings r
                                  inner join movies m on r.movieId = m.id
                                  where userId = @userId
                                  """, new { userId = userId }, cancellationToken: token));
    }
}