using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using URLShortener.Core.Aggregates;
using URLShortener.Core.Exceptions;
using URLShortener.Core.Helpers.Interfaces;
using URLShortener.Core.Repositories;
using URLShortener.Core.Services.Interfaces;
using URLShortener.Shared.DTOS.Input;
using URLShortener.Shared.DTOS.Output;

namespace URLShortener.Core.Services.Implementations;

internal class UrlShortenerService(IUrlShortenerRepository repository, IIdGeneratorHelper idGeneratorHelper, ILogger<UrlShortenerService> logger) : IUrlShortenerService
{
    private static readonly Regex IdRegex = new(@"^[a-zA-Z0-9]+$", RegexOptions.Compiled);

    public async Task<string?> GetById(string id)
    {
        try
        {
            var entity = await repository.GetById(id);
            return entity?.OriginalUrl;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to get shortened URL by ID");
            throw new UrlRetrievalFailedException();
        }
    }

    public async Task<ShortenedUrlDto> Create(CreateShortenedUrlDto dto)
    {
        ShortenedUrlAggregate aggregate;

        if (!string.IsNullOrEmpty(dto.Id))
        {
            if (!IdRegex.IsMatch(dto.Id))
            {
                throw new IdPatternNotMatchedException(dto.Id);
            }

            if (await repository.GetById(dto.Id) is not null)
            {
                throw new IdAlreadyExistsException(dto.Id);
            }

            aggregate = new ShortenedUrlAggregate(dto);
        }
        else
        {
            string id = await idGeneratorHelper.GenerateId();
            aggregate = new ShortenedUrlAggregate(id, dto);
        }

        try
        {
            await repository.Create(aggregate.ToEntity());
            return new ShortenedUrlDto(aggregate.Id, aggregate.OriginalUrl);
        }

        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create shortened URL");
            throw new UrlCreationFailedException();
        }
    }

    public async Task Delete(string id)
    {
        var entity = await repository.GetById(id);

        if (entity is null)
        {
            throw new EntityNotFoundException(id);
        }

        try
        {
            await repository.Delete(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete shortened URL");
            throw new UrlDeletionFailedException();
        }
    }
}