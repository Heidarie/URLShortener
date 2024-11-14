using System.Runtime.CompilerServices;
using URLShortener.Shared.DTOS.Input;
using URLShortener.Shared.DTOS.Output;

[assembly: InternalsVisibleTo("URLShortener.Core")]
[assembly: InternalsVisibleTo("URLShortener")]
[assembly: InternalsVisibleTo("URLShortener.UnitTests")]
[assembly: InternalsVisibleTo("URLShortener.API.Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace URLShortener.Abstractions.Services.Interfaces;

internal interface IUrlShortenerService
{
    Task<string?> GetById(string id);
    Task<ShortenedUrlDto> Create(CreateShortenedUrlDto dto);
    Task Delete(string id);
}