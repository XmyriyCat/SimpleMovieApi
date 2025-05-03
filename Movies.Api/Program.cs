using Movies.Api.Mapping;
using Movies.Application;
using Movies.Application.Database;

namespace Movies.Api;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration;

        builder.Services.AddApplication();
        builder.Services.AddDatabase(config["Database:ConnectionString"]!);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseMiddleware<ValidationMappingMiddleware>();

        app.MapControllers();

        var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
        await dbInitializer.InitializeAsync();

        await app.RunAsync();
    }
}