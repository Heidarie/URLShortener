using System.Runtime.CompilerServices;
using URLShortener.Core.Entities;

[assembly: InternalsVisibleTo("URLShortener.UnitTests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace URLShortener.Core.Repositories;

internal interface IUrlShortenerRepository
{
    Task<ShortenedUrl?> GetById(string id);
    Task Create(ShortenedUrl dto);
    Task Update(ShortenedUrl dto);
    Task Delete(ShortenedUrl dto);
}