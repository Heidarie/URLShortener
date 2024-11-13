using URLShortener.Core.Entities;

namespace URLShortener.Core.Repositories;

internal interface IUrlShortenerRepository
{
    Task<ShortenedUrl?> GetById(string id);
    Task Create(ShortenedUrl dto);
    Task Delete(ShortenedUrl dto);
}