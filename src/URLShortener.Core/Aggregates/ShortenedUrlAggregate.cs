using URLShortener.Core.Entities;
using URLShortener.Shared.DTOS.Input;

namespace URLShortener.Core.Aggregates;

internal class ShortenedUrlAggregate
{
    public string Id { get; private set; }
    public string OriginalUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    public ShortenedUrlAggregate(CreateShortenedUrlDto dto)
    {
        Id = dto.Id;
        OriginalUrl = dto.OriginalUrl;
        ExpiresAt = dto.ExpiresAt;
        CreatedAt = DateTime.UtcNow;
    }

    public ShortenedUrlAggregate(ShortenedUrl entity)
    {
        Id = entity.Id;
        OriginalUrl = entity.OriginalUrl;
        ExpiresAt = entity.ExpiresAt;
        CreatedAt = DateTime.UtcNow;
    }

    public ShortenedUrlAggregate(string id, CreateShortenedUrlDto dto)
    {
        Id = id;
        OriginalUrl = dto.OriginalUrl;
        ExpiresAt = dto.ExpiresAt;
        CreatedAt = DateTime.UtcNow;
    }

    public void SetNewUrl(string originalUrl)
    {
        OriginalUrl = originalUrl;
    }

    public ShortenedUrl ToEntity()
    {
        return new ShortenedUrl
        {
            Id = Id,
            OriginalUrl = OriginalUrl,
            ExpiresAt = ExpiresAt,
            CreatedAt = CreatedAt
        };
    }
}