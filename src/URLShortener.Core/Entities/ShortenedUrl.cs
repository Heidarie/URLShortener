namespace URLShortener.Core.Entities;

public class ShortenedUrl
{
    public string Id { get; set; } = string.Empty;
    public string OriginalUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}