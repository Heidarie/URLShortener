using URLShortener.Shared.DTOS.Input;
using URLShortener.Shared.DTOS.Output;

namespace URLShortener.Core.Services.Interfaces;

internal interface IUrlShortenerService
{
    Task<string?> GetById(string id);
    Task<ShortenedUrlDto> Create(CreateShortenedUrlDto dto);
    Task Delete(string id);
}