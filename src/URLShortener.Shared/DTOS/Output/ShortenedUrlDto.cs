namespace URLShortener.Shared.DTOS.Output;

/// <summary>
/// DTO for shortened URL.
/// </summary>
/// <param name="Id">Id of the URL.</param>
/// <param name="OriginalUrl">Original URL.</param>
/// <param name="CreatedAt">Created at.</param>
/// <param name="ExpiresAt">Expires at.</param>
public record ShortenedUrlDto(string Id, string OriginalUrl);