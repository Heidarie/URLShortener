using Microsoft.EntityFrameworkCore;
using URLShortener.Core.Entities;

namespace URLShortener.Core.DAL;

public class URLShortenerDbContext : DbContext
{
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

    public URLShortenerDbContext(DbContextOptions<URLShortenerDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("shortenedUrls");
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}