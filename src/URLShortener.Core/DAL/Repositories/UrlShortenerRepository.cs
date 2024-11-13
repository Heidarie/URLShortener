using URLShortener.Core.Entities;
using URLShortener.Core.Repositories;

namespace URLShortener.Core.DAL.Repositories;

internal class UrlShortenerRepository(URLShortenerDbContext context) : IUrlShortenerRepository
{
    public async Task<ShortenedUrl?> GetById(string id) => await context.ShortenedUrls.FindAsync(id);

    public async Task Create(ShortenedUrl dto)
    {
        await context.AddAsync(dto);
        await context.SaveChangesAsync();
    }

    public async Task Delete(ShortenedUrl dto)
    {
        context.ShortenedUrls.Remove(dto);
        await context.SaveChangesAsync();
    }
}