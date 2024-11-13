namespace URLShortener.Shared.DTOS.Input;

public class CreateShortenedUrlDto
{
    public string Id { get; set; } = string.Empty;

    public string OriginalUrl { get; set; } = string.Empty;

    public DateTime? ExpiresAt { get; set; }
}