using Microsoft.Extensions.Logging;
using Moq;
using URLShortener.Abstractions.Services.Interfaces;
using URLShortener.Core.Aggregates;
using URLShortener.Core.Entities;
using URLShortener.Core.Exceptions;
using URLShortener.Core.Helpers.Interfaces;
using URLShortener.Core.Repositories;
using URLShortener.Core.Services.Implementations;
using URLShortener.Shared.DTOS.Input;

public class UrlShortenerServiceTests
{
    private readonly Mock<IUrlShortenerRepository> _repositoryMock;
    private readonly Mock<IIdGeneratorHelper> _idGeneratorHelperMock;
    private readonly Mock<ILogger<UrlShortenerService>> _loggerMock;
    private readonly IUrlShortenerService _urlShortenerService;

    public UrlShortenerServiceTests()
    {
        _repositoryMock = new Mock<IUrlShortenerRepository>();
        _idGeneratorHelperMock = new Mock<IIdGeneratorHelper>();
        _loggerMock = new Mock<ILogger<UrlShortenerService>>();
        _urlShortenerService = new UrlShortenerService(_repositoryMock.Object, _idGeneratorHelperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetById_ShouldReturnUrl_WhenIdExists()
    {
        // Arrange
        var id = "testId";
        var originalUrl = "http://example.com";
        var entity = new ShortenedUrl() { Id = id, OriginalUrl = originalUrl, CreatedAt = DateTime.UtcNow };
        _repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(entity);

        // Act
        var result = await _urlShortenerService.GetById(id);

        // Assert
        Assert.Equal(originalUrl, result);
    }

    [Fact]
    public async Task GetById_ShouldReturnNull_WhenIdDoesNotExist()
    {
        // Arrange
        var id = "nonExistentId";
        _repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(It.IsAny<ShortenedUrl>());

        // Act
        var result = await _urlShortenerService.GetById(id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Create_ShouldReturnShortenedUrlDto_WhenUrlIsCreated()
    {
        // Arrange
        var dto = new CreateShortenedUrlDto { OriginalUrl = "http://example.com" };
        var id = "generatedId";
        _idGeneratorHelperMock.Setup(helper => helper.GenerateId(6)).ReturnsAsync(id);
        _repositoryMock.Setup(repo => repo.Create(It.IsAny<ShortenedUrl>())).Returns(Task.CompletedTask);

        // Act
        var result = await _urlShortenerService.Create(dto);

        // Assert
        Assert.Equal(id, result.Id);
        Assert.Equal(dto.OriginalUrl, result.OriginalUrl);
    }

    [Fact]
    public async Task Create_ShouldThrowIdAlreadyExistsException_WhenIdAlreadyExists()
    {
        // Arrange
        var dto = new CreateShortenedUrlDto { Id = "existingId", OriginalUrl = "http://example.com" };
        var entity = new ShortenedUrl() { Id = "existingId", OriginalUrl = "http://example.com", CreatedAt = DateTime.UtcNow };
        _repositoryMock.Setup(repo => repo.GetById(dto.Id)).ReturnsAsync(entity);

        // Act & Assert
        await Assert.ThrowsAsync<IdAlreadyExistsException>(() => _urlShortenerService.Create(dto));
    }

    [Fact]
    public async Task Create_ShouldThrowIdPatternNotMatchedException_WhenPatternNotMatched()
    {
        // Arrange
        var dto = new CreateShortenedUrlDto { Id = "---", OriginalUrl = "http://example.com" };
        var entity = new ShortenedUrl() { Id = "existingId", OriginalUrl = "http://example.com", CreatedAt = DateTime.UtcNow };
        _repositoryMock.Setup(repo => repo.GetById(dto.Id)).ReturnsAsync(entity);

        // Act & Assert
        await Assert.ThrowsAsync<IdPatternNotMatchedException>(() => _urlShortenerService.Create(dto));
    }

    [Fact]
    public async Task Delete_ShouldThrowEntityNotFoundException_WhenIdDoesNotExist()
    {
        // Arrange
        var id = "nonExistentId";
        _repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(It.IsAny<ShortenedUrl>());

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _urlShortenerService.Delete(id));
    }

    [Fact]
    public async Task Delete_ShouldCallRepositoryDelete_WhenIdExists()
    {
        // Arrange
        var id = "existingId";
        var entity = new ShortenedUrl() { Id = id, OriginalUrl = "http://example.com", CreatedAt = DateTime.UtcNow };
        _repositoryMock.Setup(repo => repo.GetById(id)).ReturnsAsync(entity);
        _repositoryMock.Setup(repo => repo.Delete(entity)).Returns(Task.CompletedTask);

        // Act
        await _urlShortenerService.Delete(id);

        // Assert
        _repositoryMock.Verify(repo => repo.Delete(entity), Times.Once);
    }
}
