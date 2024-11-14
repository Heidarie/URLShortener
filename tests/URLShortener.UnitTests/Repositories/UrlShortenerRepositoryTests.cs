using Microsoft.EntityFrameworkCore;
using Moq;
using System.Threading.Tasks;
using URLShortener.Core.DAL;
using URLShortener.Core.DAL.Repositories;
using URLShortener.Core.Entities;
using URLShortener.Core.Repositories;
using Xunit;

public class UrlShortenerRepositoryTests
{
    private readonly DbContextOptions<URLShortenerDbContext> _dbContextOptions;
    private readonly URLShortenerDbContext _context;
    private readonly IUrlShortenerRepository _repository;

    public UrlShortenerRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<URLShortenerDbContext>()
            .UseInMemoryDatabase(databaseName: "UrlShortenerTestDb")
            .Options;
        _context = new URLShortenerDbContext(_dbContextOptions);
        _repository = new UrlShortenerRepository(_context);
    }

    [Fact]
    public async Task GetById_ShouldReturnShortenedUrl_WhenIdExists()
    {
        // Arrange
        var id = "testId";
        var originalUrl = "http://example.com";
        var entity = new ShortenedUrl { Id = id, OriginalUrl = originalUrl };
        await _context.ShortenedUrls.AddAsync(entity);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetById(id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(originalUrl, result.OriginalUrl);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var id = "nonExistentId";

        // Act
        var result = await _repository.GetById(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ShouldAddShortenedUrl()
    {
        // Arrange
        var entity = new ShortenedUrl { Id = "newId", OriginalUrl = "http://example.com" };

        // Act
        await _repository.Create(entity);
        var result = await _context.ShortenedUrls.FindAsync(entity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(entity.OriginalUrl, result.OriginalUrl);
    }

    [Fact]
    public async Task Update_ShouldModifyShortenedUrl()
    {
        // Arrange
        var entity = new ShortenedUrl { Id = "updateId", OriginalUrl = "http://example.com" };
        await _context.ShortenedUrls.AddAsync(entity);
        await _context.SaveChangesAsync();

        // Act
        entity.OriginalUrl = "http://updated.com";
        await _repository.Update(entity);
        var result = await _context.ShortenedUrls.FindAsync(entity.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("http://updated.com", result.OriginalUrl);
    }

    [Fact]
    public async Task Delete_ShouldRemoveShortenedUrl()
    {
        // Arrange
        var entity = new ShortenedUrl { Id = "deleteId", OriginalUrl = "http://example.com" };
        await _context.ShortenedUrls.AddAsync(entity);
        await _context.SaveChangesAsync();

        // Act
        await _repository.Delete(entity);
        var result = await _context.ShortenedUrls.FindAsync(entity.Id);

        // Assert
        Assert.Null(result);
    }
}
