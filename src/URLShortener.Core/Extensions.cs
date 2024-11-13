using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;
using URLShortener.Core.DAL;
using URLShortener.Core.DAL.Repositories;
using URLShortener.Core.Helpers.Implementations;
using URLShortener.Core.Helpers.Interfaces;
using URLShortener.Core.Repositories;
using URLShortener.Core.Services.Implementations;
using URLShortener.Core.Services.Interfaces;
using URLShortener.Infrastracture.Postgres;

[assembly: InternalsVisibleTo("URLShortener")]
namespace URLShortener.Core;

internal static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services
            .AddScoped<IUrlShortenerRepository, UrlShortenerRepository>()
            .AddScoped<IUrlShortenerService, UrlShortenerService>()
            .AddScoped<IIdGeneratorHelper, IdGeneratorHelper>()
            .AddPostgres<URLShortenerDbContext>();

        return services;
    }

    public static IServiceCollection MigrateDatabase(this IServiceCollection services)
    {
        using var serviceProvider = services.BuildServiceProvider();
        var dbContext = serviceProvider.GetRequiredService<URLShortenerDbContext>();
        dbContext.Database.Migrate();

        return services;
    }
}