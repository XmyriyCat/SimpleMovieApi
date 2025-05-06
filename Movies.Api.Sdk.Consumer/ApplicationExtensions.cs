using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace Movies.Api.Sdk.Consumer;

public static class ApplicationExtensions
{
    public static void ConfigureRefitClient(this IServiceCollection services)
    {
        services.AddRefitClient<IMoviesApi>(x => new RefitSettings
            {
                AuthorizationHeaderValueGetter = delegate
                {
                    return Task.FromResult(
                        "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiJjMGZlZmNiMi1mY2Y2LTQzZTM" +
                        "tYjU0Yi1hZjM5ZGU3MzAxMWIiLCJzdWIiOiJwYXVsQGFsZXNocGF1bC5jb20iLCJlbWFpbCI" +
                        "6InBhdWxAYWxlc2hwYXVsLmNvbSIsInVzZXJpZCI6Ijg3MDkzMDVhLTI2Y2EtNDZkNS1hZGI" +
                        "0LThjYWMyNjM2M2VjOSIsImFkbWluIjoidHJ1ZSIsInRydXN0ZWRfbWVtYmVyIjoidHJ1ZSI" +
                        "sIm5iZiI6IjE3NDYyODUwOTAiLCJleHAiOiIxNzUxNTU1NDkwIiwiaWF0IjoiMTc0NjI4NTA" +
                        "5MCIsImlzcyI6Imh0dHBzOi8vaWQuYWxlc2hwYXVsLmNvbSIsImF1ZCI6Imh0dHBzOi8vbW9" +
                        "2aWVzLmFsZXNocGF1bC5jb20ifQ.aD4yyw5C7Lu0DUIXejTJAHdWDvKmbXnKnpXBwtgDxcE");
                }
            })
            .ConfigureHttpClient(x =>
                x.BaseAddress = new Uri("https://localhost:5000"));
    }
}